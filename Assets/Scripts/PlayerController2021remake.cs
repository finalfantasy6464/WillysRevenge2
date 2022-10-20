using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using TMPro;
using System;

public class PlayerController2021remake : MonoBehaviour, IPausable
{
    public PlayerCollision playerCollision;
    //Otherobjects
    public bool shieldactive = false;
    public GameObject Shield;
    public GameObject PlayerSprite;
    bool Switch1Pressed = false;
    bool Switch2Pressed = false;
    private Vector3 doormove = new Vector3(0, 0.72f, 0);

    public int MoveCounter;

    public bool dirlock = false;
    public bool gotgold = false;
    public bool delaylock = false;
    public bool isHeadJumping = false;

    public Sprite[] skinSprites;
    public Sprite[] tailSprites;
 
    public int enabledSegmentAmount;
    public Transform lastEnabledSegment => taillist[enabledSegmentAmount - 1].transform;
    private float pelletgap;
    Rigidbody2D rb;

    SpriteRenderer spriteRenderer;

    PauseControl pause;

    public PlayerInput playerInput;

    //Segments
    public List<GameObject> taillist = new List<GameObject>();
    public List<SpriteRenderer> tailListRenderers = new List<SpriteRenderer>();
    public List<CircleCollider2D> tailListColliders = new List<CircleCollider2D>();
    private List<Vector3> PositionHistory = new List<Vector3>();
    public List<float> AlphaHistory = new List<float>();
    public List<Vector3> slowdownPositionCache = new List<Vector3>();
    public int followDelay;
    public int speedIncrease;
    public bool debugShowFromStart;
    public Sprite[] SegmentSprites;
    public int offsetFromHead;
    public int LatestSegmentDelay;
    public int SegmentDelaySleep = 5;
    public int SegmentDelaySleepcache;
    public int SegmentDelaySleepCounter;
    public int SegmentHistorySkipCounter;
    public int SegmentHistorySkipTimer;
    public bool ate;
    public bool isSegmentJumping;
    public Vector3 LastLandPosition;

    // movement
    public int presentdir;
    public Vector2 moveValue = Vector2.zero;
    public Vector2 moveValueCache = Vector2.zero;
    public Vector2 mainDirection = Vector2.zero;
    public Vector3 direction = Vector3.zero;
    public Vector3 finalMovementVector = Vector3.zero;
    public Vector3 shiftVector = Vector3.zero;
    public Vector3 corruptionDirectionCache = Vector3.zero;

    private bool moving;
    private float rotationAngle;
    public bool canmove = true;
    public bool directionreset = false;

    public float basespeed = 1.8f;
    public float speedMultiplier = 1f;
    public float finalspeed;

    private float pelletmagnitude = 0.08f;

    public float pelletno = 0.0f;

    private int Scorecount;

    private int numberofpickups;

    LevelCanvas canvas;
    public TextMeshProUGUI countText;
    public TextMeshProUGUI speedText;
    public Image GoldenPellet;

    [HideInInspector] public UnityEvent onEatPellet;
    [HideInInspector] public UnityEvent onGoldenPellet;
    [HideInInspector] public UnityEvent onSlidingDoorOpen;
    [HideInInspector] public UnityEvent onSlidingDoorClose;
    [HideInInspector] public UnityEvent onCollectShield;

    public bool isPaused { get ; set ; }

    void Awake()
    {
        onEatPellet = new UnityEvent();
        onGoldenPellet = new UnityEvent();
        onSlidingDoorOpen = new UnityEvent();
        onSlidingDoorClose = new UnityEvent();
        onCollectShield = new UnityEvent();

        for (int i = 0; i < 80; i++) 
        {
            slowdownPositionCache.Add(new Vector3(99,99,99));
        }

        SegmentDelaySleepcache = SegmentDelaySleep;
    }

    void Start()
    {
        canvas = GameObject.FindObjectOfType<LevelCanvas>();
        countText = canvas.countText;
        GoldenPellet = canvas.goldenImage;
        speedText = canvas.speedText;
        pause = GameControl.control.GetComponent<PauseControl>();


        basespeed = basespeed * speedMultiplier;
        Scorecount = 0;
        dirlock = false;
        SetCountMax();
        SetCountText();
        SetSpeedText();
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = PlayerSprite.GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = skinSprites[GameControl.control.currentCharacterSprite];
        SetTailPieceSprite();

        for (int i = 0; i < taillist.Count; i++)
        {
            if (taillist[i].activeInHierarchy)
                enabledSegmentAmount++;
            else
                break;
        }

        if (debugShowFromStart == true)
        {
            foreach (GameObject segment in taillist)
            {
                segment.SetActive(true);
            }
        }
    }

    void Update()
    {
        if(!isPaused)
            UnPausedUpdate();
    }

    void UpdateHistoryLists()
    {
        PositionHistory.Insert(0, transform.position);
        AlphaHistory.Insert(0, spriteRenderer.color.a);

        if(PositionHistory.Count > 1000)
        {
            PositionHistory.RemoveRange(1000, PositionHistory.Count - 1000);
        }

        if (AlphaHistory.Count > 1000)
        {
            AlphaHistory.RemoveRange(1000, AlphaHistory.Count - 1000);
        }

    }

    void OnMove(InputValue value)
    {
        moveValueCache = moveValue;
        moveValue = value.Get<Vector2>();
        float Angle = VectorToAngle(moveValue);

        // if (Angle > 35 && Angle < 55 || Angle > 125 && Angle < 145 || Angle > 215 && Angle < 235 || Angle > 305 && Angle < 325)
        // {
        //     return;
        // }

        if (moveValueCache.y == 0 && moveValueCache.x == 0)
        {
            if (moveValue.x != 0)
            {
                SetMoveDirection(moveValue.x > 0 ? 1 : -1, 0, moveValue.x > 0 ? 0 : 180);
            }
            else if (moveValue.y != 0)
            {
                SetMoveDirection(0, moveValue.y > 0 ? 1 : -1, moveValue.y > 0 ? 90 : 270);
            }
            return;
        }

        if (moveValueCache.y != 0 && moveValue.y != 0)
        {
            if (moveValue.x != 0)
            {
                //If while going moving UP, you move left or right but not enough, don't change direction.
                if (moveValue.y > 0 && ((moveValue.x > 0 && Angle < 50) || (moveValue.x < 0 && Angle > 310)))
                    return;
                //If while going moving DOWN, you move left or right but not enough, don't change direction.                    
                else if (moveValue.y < 0 && ((moveValue.x > 0 && Angle > 130) || (moveValue.x < 0 && Angle < 230)))
                    return;

                SetMoveDirection(moveValue.x > 0 ? 1 : -1, 0, moveValue.x > 0 ? 0 : 180);
                return;
            }
            else if (moveValueCache.x != 0 && moveValue.x == 0 && moveValue.y != 0)
            {
                //If while going moving RIGHT, you move up or down but not enough, don't change direction.
                if (moveValue.x > 0 && ((moveValue.y > 0 && Angle > 40) || (moveValue.y < 0 && Angle < 140)))
                    return;
                //If while going moving LEFT, you move up or down but not enough, don't change direction.                    
                else if (moveValue.x < 0 && ((moveValue.y > 0 && Angle < 320) || (moveValue.y < 0 && Angle > 220)))
                    return;

                SetMoveDirection(0, moveValue.y > 0 ? 1 : -1, moveValue.y > 0 ? 90 : 270);
                return;
            }
        }
    }


    void OnMenu()
    {
        pause.ChangePauseState();
    }

    void OnCancellation()
    {
        if (pause.isGamePaused)
        {
            pause.ChangePauseState();
        }
    }

    void SetMoveDirection(float directionX, float directionY, float RotationAngle)
    {
        MoveCounter++;

        if(dirlock == false)
        {
            direction.x = directionX;
            direction.y = directionY;
            rotationAngle = RotationAngle;
            transform.rotation = Quaternion.Euler(0, 0, rotationAngle);
        }

        finalspeed = basespeed + (pelletmagnitude * (pelletno * speedMultiplier));

        if (canmove)
        {
            if(corruptionDirectionCache != Vector3.zero)
                direction = corruptionDirectionCache;
        }
        else
        {
            direction.x = 0;
            direction.y = 0;
            finalMovementVector = Vector3.zero;
            transform.Translate(finalMovementVector);
        }
    }

    void DoMove()
    {
        finalMovementVector = shiftVector + (direction * finalspeed) * Time.deltaTime;
        transform.position += finalMovementVector;
    }

    public float VectorToAngle(Vector2 vector2)
    {
        if (vector2.x < 0)
        {
           return 360 - (Mathf.Atan2(vector2.x, vector2.y) * Mathf.Rad2Deg * -1);
        }
        else
        {
            return (Mathf.Atan2(vector2.x, vector2.y) * Mathf.Rad2Deg);
        }
    }

    void UpdateSegmentSprite(Sprite segmentsprite)
    {
        SpriteRenderer currentrenderer;
        SpriteRenderer previousrenderer;

        for (int i = taillist.Count - 1; i > 0; i--)
        {
            if (!taillist[i].activeInHierarchy)
            {
                continue;
            }
            currentrenderer = taillist[i].GetComponent<SpriteRenderer>();
            previousrenderer = taillist[i - 1].GetComponent<SpriteRenderer>();
            currentrenderer.sprite = previousrenderer.sprite;
        }

        taillist[0].GetComponent<SpriteRenderer>().sprite = segmentsprite;
    }

    void SetTailPieceSprite()
    {
        SegmentSprites[0] = tailSprites[GameControl.control.currentCharacterSprite];
        foreach(GameObject tail in taillist)
        {
            tail.GetComponent<SpriteRenderer>().sprite = SegmentSprites[0];
        }
    }

    void UpdateSegmentTint()
    {
        SpriteRenderer currentrenderer;
        
        for (int i = 0; i < taillist.Count; i++)
        {
            currentrenderer = taillist[i].GetComponent<SpriteRenderer>();
            currentrenderer.color = new Color(1f - (i * 0.007f), 1f - (i * 0.007f), 1f - (i * 0.007f), 1f);
           
            if (!taillist[i].activeInHierarchy)
            {
                break;
            }
        }
    }

    void UpdateSegmentAlpha()
    {
        SpriteRenderer currentrenderer;

        for (int i = 0; i < taillist.Count; i++)
        {
            if (!taillist[i].activeInHierarchy)
            {
                break;
            }

            currentrenderer = taillist[i].GetComponent<SpriteRenderer>();
            if (AlphaHistory.Count > offsetFromHead + (followDelay * i))
            {
                currentrenderer.color = new Color(currentrenderer.color.r, currentrenderer.color.b, currentrenderer.color.g, AlphaHistory[offsetFromHead + (followDelay * i)]);
            }
        }
    }

    void ShowNextSegment(Sprite segmentsprite, int speedIncrease)
    {
        SegmentHistorySkipCounter = offsetFromHead;

        if (ate)
        {
            LatestSegmentDelay = 1;
            SegmentDelaySleepCounter = SegmentDelaySleep;
        }
        ate = true;
        LatestSegmentDelay = offsetFromHead;
        SegmentDelaySleep = SegmentDelaySleepcache;

        for (int i = 0; i < taillist.Count; i++)
        {
            if (!taillist[i].activeInHierarchy)
            {
                taillist[i].SetActive(true);
                enabledSegmentAmount++;
                break;
            }
        }
        UpdateSegmentSprite(segmentsprite);
        UpdateSegmentTint();
    }

    void UpdateSegmentsPosition()
    {
        int delay;

        for (int i = 0; i < taillist.Count; i++)
        {
            if (!taillist[i].activeInHierarchy)
                break;

            delay = offsetFromHead + (followDelay * i);

            taillist[i].transform.rotation = Quaternion.Euler(0, 0, 0);
            if (PositionHistory.Count > delay)
            {
                taillist[i].transform.position = PositionHistory[delay];
            }

            if (i == taillist.Count - 1 && taillist[i].transform.position == LastLandPosition)
            {
                isSegmentJumping = false;
            }
        }
    }

    void UpdateSegmentDelay()
    {
        if (!ate)
          return;

        if (SegmentDelaySleepCounter < SegmentDelaySleep)
        {
            SegmentDelaySleepCounter++;
        }
        else
        {
            LatestSegmentDelay--;
            if (LatestSegmentDelay == 0)
            {
                ate = false;
                LatestSegmentDelay = offsetFromHead;
                SegmentDelaySleep = SegmentDelaySleepcache;
            }
            else
            {
                SegmentDelaySleep = Mathf.Max(0, SegmentDelaySleep - 1);
            }
            SegmentDelaySleepCounter = 0;
        }
    }

    void UpdateSegmentOrder()
    {
        SpriteRenderer currentrenderer;

        for (int i = 0; i < taillist.Count; i++)
        {
             currentrenderer = taillist[i].GetComponent<SpriteRenderer>();
             currentrenderer.sortingOrder = -i;

            if (isHeadJumping || isSegmentJumping)
            {
             currentrenderer.sortingOrder += (int)(taillist[i].transform.localScale.x * 10.0f);
            }

            if (!taillist[i].activeInHierarchy)
            {
                break;
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Pickup"))
        {
            onEatPellet.Invoke();
            Destroy(other.gameObject);
            pelletgap = 0;
            pelletno += 1;
            Scorecount = Scorecount + 1;
            
            SetCountText();
            if (!debugShowFromStart)
            {
                ShowNextSegment(SegmentSprites[0], 1);
            }
        }

        if (other.gameObject.CompareTag("GoldenPickup"))
        {
            onGoldenPellet.Invoke();
            Destroy(other.gameObject);
            GoldenPellet.enabled = true;
            gotgold = true;
            if (!debugShowFromStart)
            {
                ShowNextSegment(SegmentSprites[1], 0);
            }
        }

        if (other.gameObject.tag == "SuperPickup")
        {
            onEatPellet.Invoke();
            Destroy(other.gameObject);
            pelletno += 40;
            if (!debugShowFromStart)
            {
               ShowNextSegment(SegmentSprites[4], 40);
            }
        }

        if (other.gameObject.CompareTag("AntiPickup"))
        {
            onEatPellet.Invoke();
            Destroy(other.gameObject);
            pelletno = Mathf.Max(0, pelletno - 40);

            if (!debugShowFromStart)
            {
                ShowNextSegment(SegmentSprites[2], -40);
            }
        }

        if (other.gameObject.CompareTag("SlowPickup"))
        {
            onEatPellet.Invoke();
            Destroy(other.gameObject);
            pelletno = Mathf.Max(0, pelletno - 5);
            if (!debugShowFromStart)
            {
                ShowNextSegment(SegmentSprites[3], -5);
            }
        }


        if (other.gameObject.CompareTag("Switch"))
        {
            if (Switch1Pressed == false)
            {
                Switch1Pressed = true;

                Switch2Pressed = false;
            }
        }
        if (other.gameObject.CompareTag("Switch2"))
        {
            if (Switch2Pressed == false)
            {
                Switch1Pressed = false;

                Switch2Pressed = true;
            }
        }

        if (other.gameObject.CompareTag("Shield"))
        {
            GameObject Shieldnew = new GameObject();

            if(shieldactive != true)
            {
                onCollectShield.Invoke();
                Vector3 s = transform.position;
                Destroy(other.gameObject);
                Shieldnew = Instantiate(Shield, s, Quaternion.identity);
                PauseControl.TryAddPausable(Shieldnew);
                shieldactive = true;
            }
            else
            {
                Shieldnew = GameObject.FindGameObjectWithTag("ActiveShield");
                Shieldnew.GetComponent<Shield>().shieldtimer += 5.0f;
                Destroy(other.gameObject);
            }
        }
}

    void SetCountText()
    {
        countText.text = "Pickups: " + Scorecount.ToString() + " / " + numberofpickups.ToString();
    }

    void SetSpeedText()
    {
        if(speedText != null)
        {
            if (direction == Vector3.zero)
            {
                speedText.text = "Speed: " + 0 + " Mph";
            }
            else
            {
                speedText.text = "Speed: " + finalspeed.ToString() + " MPH";
            }
        }
    }

    void SetCountMax()
    {
        GameObject[] totalpickups = GameObject.FindGameObjectsWithTag("Pickup");
        numberofpickups = totalpickups.Length;
    }

    void DebugOptionsCheck()
    {
        if (Input.GetKey(KeyCode.N))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }

        if (Input.GetKey(KeyCode.B))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
        }
    }

    void OnReset()
    {
        if(!isPaused)
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void OnPause()
    {
        isPaused = true;
    }

    public void OnUnpause()
    {
        isPaused = false;
    }

    public void UnPausedUpdate()
    {
        if(!playerCollision.dyingByCorruption)
        {
            MoveCounter = 0;
            DoMove();
            UpdateSegmentAlpha();
        }
        
        SetSpeedText();
        UpdateSegmentsPosition();
        UpdateSegmentDelay();
        UpdateSegmentOrder();
        //DebugOptionsCheck();
        UpdateHistoryLists();
        this.pelletgap += Time.deltaTime;
    }

    public void LateUpdate()
    {
        Debug.Log(MoveCounter);
    }

    public void OnDestroy()
    {
        PauseControl.TryRemovePausable(gameObject);
    }
}