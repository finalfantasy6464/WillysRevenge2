using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathArrowsDisplay : MonoBehaviour
{
    public Transform[] arrows;
    public float animTime;
    public float animCounter;

    public Color NextColour;
    public Color PreviousColour;
    
    List<Vector3> arrowStarts;
    List<SpriteRenderer> arrowRenderers;
    List<Color> arrowBaseColours;
    List<Color> arrowClearColours;

    bool isPlayerOnPin;

    void Start()
    {
        arrowStarts = new List<Vector3>();
        arrowRenderers = new List<SpriteRenderer>();
        arrowBaseColours = new List<Color>();
        arrowClearColours = new List<Color>();

        AssignArrowColours();

        for (int i = 0; i < arrows.Length; i++)
        {
            arrowStarts.Add(arrows[i].position);
            arrowRenderers.Add(arrows[i].GetComponent<SpriteRenderer>());
            arrowBaseColours.Add(arrows[i].GetComponent<SpriteRenderer>().color);
            arrowClearColours.Add(new Color(
                    arrowBaseColours[i].r, arrowBaseColours[i].g, arrowBaseColours[i].b, 0f));
        }
    }

    void LateUpdate()
    {
        if(!isPlayerOnPin)
            return;

        animCounter += Time.deltaTime;

        if(animCounter > animTime)
            animCounter = 0f;
        
        for (int i = 0; i < arrows.Length; i++)
        {
            if(!arrows[i].gameObject.activeInHierarchy)
                continue;

            arrows[i].position = arrowStarts[i] + arrows[i].right
                    * Mathf.Lerp(0f, 0.25f, Mathf.Min(1f, ((animCounter * 2f)/animTime)));
            arrowRenderers[i].color = Color.Lerp(
                    arrowClearColours[i], arrowBaseColours[i], ((animCounter * 2f)/animTime));
        }
    }

    void AssignArrowColours()
    {
        LevelPin pin = GetComponentInParent<LevelPin>();
        if(pin.previousDirection != PathDirection.None)
        {
            GetArrowFromDirection(pin.previousDirection)
                .GetComponent<SpriteRenderer>().color = PreviousColour;
        }
        if(pin.nextDirection != PathDirection.None)
        {
            GetArrowFromDirection(pin.nextDirection)
                .GetComponent<SpriteRenderer>().color = NextColour;
        }  
    }

    public GameObject GetArrowFromDirection(PathDirection direction)
    {
        return arrows[(int)direction - 1].gameObject;
    }

    public void ResetAnimation()
    {
        animCounter = 0f;
    }

    public void SetArrow(PathDirection direction, bool state)
    {
        GetArrowFromDirection(direction).SetActive(state);
    }

    public void SetIsPlayerOnPin(bool value)
    {
        if(isPlayerOnPin && !value)
            ResetAnimation();

        isPlayerOnPin = value;
    }
}

