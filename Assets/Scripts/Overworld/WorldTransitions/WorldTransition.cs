using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class WorldTransition : MonoBehaviour
{
    public Behaviour[] toDisable;
    public OverworldCharacter character;
    
    public bool secondTraversed;
    IEnumerator forwardRoutine;
    IEnumerator backwardRoutine;
    WorldTransitionNode nodeA;
    WorldTransitionNode nodeB;

    public Camera followingCamera;
    public Animator cameraAnim;
    public string animStateforward;
    public string animStatebackward;
    public CameraFollow followScript;

    protected Collider2D characterCollider;

    public ObjectToggle toggle;
    public ObjectToggle backwardToggle;
    public MonoBehaviour waypoint;
    public MonoBehaviour backwardswaypoint;

    protected virtual void Start()
    {
        nodeA = transform.GetChild(0).GetComponent<WorldTransitionNode>();
        nodeB = transform.GetChild(1).GetComponent<WorldTransitionNode>();
        characterCollider = character.GetComponent<Collider2D>();
    }

    public void TriggerToggleBehaviour()
    {
        if (toggle != null)
        {
            toggle.ToggleBehaviour();
        }

        if (waypoint != null)
        {
                if (waypoint is ColourWaypoints)
                {
                    ((ColourWaypoints)waypoint).WaypointBehaviour();
                }
                else if (waypoint is MoonToggle)
                {
                    ((MoonToggle)waypoint).WaypointBehaviour();
                }
                else if (waypoint is MoonToggle)
                {
                    ((AlienToggle)waypoint).WaypointBehaviour();
                }
        }
    }

    public void TriggerToggleBehaviourBackward()
    {
        if (backwardToggle != null)
        {
            backwardToggle.ToggleBehaviour();
        }

        if (backwardswaypoint != null)
        {
            if (backwardswaypoint is ColourWaypoints)
            {
                ((ColourWaypoints)backwardswaypoint).WaypointBehaviour();
            }
            else if (backwardswaypoint is MoonToggle)
            {
                ((MoonToggle)backwardswaypoint).WaypointBehaviour();
            }
            else if (backwardswaypoint is MoonToggle)
            {
                ((AlienToggle)backwardswaypoint).WaypointBehaviour();
            }
        }
    }

    void OnTransitionStart()
    {
        character.currentWorldTransition = this;
        foreach (Behaviour b in toDisable)
        {
            b.enabled = false;
            //Or other stopping code here, implement Interface if necessary.
        }
    }

    void RoutineOverwriteCheck(IEnumerator routine)
    {
        if(routine == null)
            return;

        StopCoroutine(forwardRoutine);
        routine = null;
        Debug.LogWarning("Coroutine hadn't finished, had to be stopped");
    }

    protected void OnEnterAForward()
    {
        RoutineOverwriteCheck(forwardRoutine);
        OnTransitionStart();

        character.isIgnoringPath = true;
        characterCollider.enabled = false;
        secondTraversed = true;
        cameraAnim.GetComponent<CameraFollow>().enabled = false;
        cameraAnim.enabled = true;
        cameraAnim.Play(animStateforward);
        StartCoroutine(ForwardRoutine());
    }

    protected void OnEnterBForward()
    {
        secondTraversed = false;
    }

    protected void OnEnterABackward()
    {
        secondTraversed = false;
    }

    protected void OnEnterBBackward()
    {
        RoutineOverwriteCheck(backwardRoutine);
        OnTransitionStart();

        character.isIgnoringPath = true;
        characterCollider.enabled = false;
        secondTraversed = true;
        cameraAnim.GetComponent<CameraFollow>().enabled = false;
        cameraAnim.enabled = true;
        cameraAnim.Play(animStatebackward);
        StartCoroutine(BackwardRoutine());
    }

    protected virtual void OnTransitionEnd()
    {
        foreach (Behaviour b in toDisable)
            b.enabled = true;

        characterCollider.enabled = true;

        cameraAnim.enabled = false;
        cameraAnim.GetComponent<CameraFollow>().enabled = true;
    }

    public void OnEnterNode(WorldTransitionNode node)
    {
        if(node == nodeA)
        {
            if(!secondTraversed)
                OnEnterAForward();
            else
                OnEnterABackward();
        } else if (node == nodeB)
        {
            if(!secondTraversed)
                OnEnterBBackward();
            else
                OnEnterBForward();
        } else
        {
            Debug.LogError("Unrecognized node order in World Transition");
        }
    }

    protected abstract IEnumerator ForwardRoutine();
    protected abstract IEnumerator BackwardRoutine();
}