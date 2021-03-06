using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SegmentCollision : MonoBehaviour
{
    public PlayerController2021Arena arena;

    void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.gameObject.tag == "Enemy3" && gameObject.activeInHierarchy)
        {
            gameObject.SetActive(false);
            arena.pelletno--;
            arena.SegmentSetter();
        }
    }
    void OnCollisionEnter2D(Collision2D coll)
    {
        if (coll.gameObject.tag == "Enemy" && gameObject.activeInHierarchy || coll.gameObject.tag == "Enemy2" && gameObject.activeInHierarchy)
        {
            gameObject.SetActive(false);
            arena.pelletno--;
            arena.SegmentSetter();
        }
    }
}
