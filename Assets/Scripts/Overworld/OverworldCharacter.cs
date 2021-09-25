using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class OverworldCharacter : MonoBehaviour
{
    public float moveSpeed;
    public bool isMoving;
    public  bool canMove;
    [Header("Live Data")]
    public NavigationPin currentPin;
    public NavigationPin targetPin;
    [HideInInspector] public UnityEvent onMove;
    IEnumerator moveRoutine;

    void Awake()
    {
        onMove = new UnityEvent();
    }

    public void Initialize(NavigationPin startPin)
    {
        SetCurrentPin((LevelPin) startPin);
        currentPin.onCharacterEnter.Invoke();
    }

    public void SetCurrentPin(LevelPin pin)
    {
        currentPin = pin;
        transform.position = pin.transform.position;
        if(moveRoutine != null)
        {
            StopCoroutine(moveRoutine);
            moveRoutine = null;
        }    
    }

    void Update()
    {
        if(canMove && !isMoving)
            MoveCheck();
    }

    private void MoveCheck()
    {
        PathDirection direction = PathDirection.None;

        if(GameInput.GetKeyDown("left"))       direction = PathDirection.Left;
        else if(GameInput.GetKeyDown("up"))    direction = PathDirection.Up;
        else if(GameInput.GetKeyDown("right")) direction = PathDirection.Right;
        else if(GameInput.GetKeyDown("down"))  direction = PathDirection.Down;

        TryFollowPath(direction);
    }

    bool TryFollowPath(PathDirection direction)
    {
        if(direction != PathDirection.None
                && currentPin.IsPathAvailable(direction, out BezierNode path))
        {
            FollowPath(path);
            onMove.Invoke();
            return true;
        }

        return false;
    }

    public void FollowPath(BezierNode path, bool instant = false)
    {
        Transform start = path.start;
        Transform end = path.end;
        bool isReturning = false;

        // Swap start and end if going back
        if(path.end == currentPin.transform)
        {
            start = path.end;
            end = path.start;
            isReturning = true;
        }

        if(instant)
        {
            transform.position = end.position;
            currentPin = end.GetComponent<LevelPin>();
            targetPin = null;
        }
        else
        {
            targetPin = end.GetComponent<LevelPin>();
            if(moveRoutine != null)
                StopCoroutine(moveRoutine);

            moveRoutine = MoveRoutine(path, isReturning);
            StartCoroutine(moveRoutine);
        }
    }   

    public IEnumerator MoveRoutine(BezierNode path, bool isReturning)
    {
        if(moveSpeed <= 0f) yield break; //to prevent infinite loops

        isMoving = true;
        currentPin.onCharacterExit.Invoke();
        float t = 0f;
        Vector2[] points = isReturning ? path.GetPointsReversed() : path.GetPoints();
        Vector2 lookDirection = Vector2.zero;
        float arcLength = BezierManager.GetLengthFromPoints(points);
        float moveStep = (moveSpeed * 0.1f) / arcLength;
        
        while(t < 1f)
        {
            t += moveStep;
            transform.position = BezierManager.GetPositionAtTime(points, t); 
            lookDirection = (Vector2)BezierManager.GetPositionAtTime(points, t + moveStep)
                    - (Vector2)transform.position;
            transform.rotation = Quaternion.Euler(
                    0, 0, Vector2.SignedAngle(Vector2.right, lookDirection));
            yield return null;
        }

        currentPin = isReturning ? path.start.GetComponent<NavigationPin>()
                : path.end.GetComponent<NavigationPin>();
        currentPin.onCharacterEnter.Invoke();

        if(currentPin is LevelPin)
        {
            targetPin = null;
            isMoving = false;
            yield break;
        }

        // When colliding with a Gate, automatically follow the right direction.
        PathDirection targetDirection = PathDirection.None;
        bool isLocked = ((GatePin)currentPin).LockCheck();

        if(isLocked && isReturning || !isLocked && !isReturning)
            targetDirection = currentPin.nextDirection;
        else if(isLocked && !isReturning || !isLocked && isReturning)
            targetDirection = currentPin.previousDirection;

        TryFollowPath(targetDirection);
    }
}
