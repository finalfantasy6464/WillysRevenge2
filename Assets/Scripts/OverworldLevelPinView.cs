using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OverworldLevelPinView : MonoBehaviour
{
    public OverworldLevelPin pin;
    public OverworldPlayer player;
    public SpriteRenderer[] orderableRenderers;
    public Animator myAnimator;
    public Collider myCollider;
    
    public AnimatorOverrideController goldOverride;

    float drawThresholdY;
    bool isPressed;

    IEnumerator Start()
    {
        drawThresholdY = myCollider.bounds.max.y;
        yield return null;
        ViewProgressCheck();
    }

    void Update()
    {
        if(isPressed && orderableRenderers[0].sortingOrder == 0)
            return;

        UpdateRenderers();
    }

    void UpdateRenderers()
    {
        for (int i = 0; i < orderableRenderers.Length; i++)
        {
            if(player.transform.position.y >= drawThresholdY)
            {
                orderableRenderers[0].sortingOrder = 20;
            }
            else
            {
                orderableRenderers[i].sortingOrder = 3 - i;
            }
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent(out OverworldPlayer player))
        {
            myAnimator.SetBool("IsPressed", true);
            orderableRenderers[0].sortingOrder = 0;
            isPressed = true;
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if(other.TryGetComponent(out OverworldPlayer player))
        {
            myAnimator.SetBool("IsPressed", false);
            isPressed = false;
        }
    }

    public void ViewProgressCheck()
    {
        if(GameControl.control.goldenpellets[pin.levelNumber])
        {
            myAnimator.SetTrigger("OnMakeGolden");
            StartCoroutine(ChangeStateRoutine(goldOverride));
        }

        if (GameControl.control.timerchallenge[pin.levelNumber])
        {
            myAnimator.SetTrigger("OnAddChallengeRim");
        }
        else if (GameControl.control.completedlevels[pin.levelNumber] && !GameControl.control.timerchallenge[pin.levelNumber])
        {
            myAnimator.SetTrigger("OnAddCompleteRim");
        }
    }

    IEnumerator ChangeStateRoutine(AnimatorOverrideController animationOverride)
    {
        yield return null;
        while(myAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1f)
            yield return null;
        myAnimator.runtimeAnimatorController = animationOverride;
        myAnimator.Play("Idle", -1);
    }
}