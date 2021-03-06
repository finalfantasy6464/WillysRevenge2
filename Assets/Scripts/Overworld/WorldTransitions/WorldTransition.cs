using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class WorldTransition : MonoBehaviour
{
    public OverworldProgressView progressViewForward;
    public OverworldProgressView progressViewBackward;
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

    protected Collider2D characterCollider;

    public OverworldCamera overworldCamera;

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

        if(progressViewForward != OverworldProgressView.None)
        {
            overworldCamera.viewToggler.Set(progressViewForward);
            GameObject gameControl = GameObject.Find("GameControl");
            gameControl.GetComponent<GameControl>().progressView = progressViewForward;
        }
    }

    public void TriggerToggleBehaviourBackward()
    {
        if(progressViewBackward != OverworldProgressView.None)
        {
            overworldCamera.viewToggler.Set(progressViewBackward);
            GameObject gameControl = GameObject.Find("GameControl");
            gameControl.GetComponent<GameControl>().progressView = progressViewBackward;
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
        overworldCamera.enabled = false;
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
        overworldCamera.enabled = true;
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
