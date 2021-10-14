using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlienToggle : MonoBehaviour
{
    public Camera mainCamera;
    public Transform Player;
    public float zoomLevel;

    public bool following;


    private void Update()
    {
        if (following)
        {
            Vector3 next = Player.transform.position;
            next.z = mainCamera.transform.position.z;
            mainCamera.transform.position = next;
        }
    }
    private void OnTriggerEnter2D(Collider2D coll)
    {
        if(coll.CompareTag("Player"))
            {
            following = true;
            mainCamera.orthographicSize = zoomLevel;
            }
    }
}
