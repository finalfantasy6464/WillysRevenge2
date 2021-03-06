using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemyMovementThreeVariantArena : MonoBehaviour, IPausable
{
	public int waypointIndex = 0;
	public float movespeed = 2f;
    public float storedmovespeed;
	public Transform[] waypoints;

    PlayerController2021Arena arena;

    public bool isPaused { get; set; }

    void Start()
    {
		transform.position = waypoints[waypointIndex].transform.position;
        storedmovespeed = movespeed;
        arena = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController2021Arena>();
	}

     void Update(){
        if (!isPaused)
            UnPausedUpdate();
	}

	void Move() {
		transform.position = Vector3.MoveTowards (transform.position, waypoints [waypointIndex].transform.position, movespeed * Time.smoothDeltaTime);

		if (transform.position == waypoints [waypointIndex].transform.position) {
			waypointIndex += 1;
		}

        if (waypointIndex >= waypoints.Length)
            Destroy(transform.parent.gameObject);
	}

    public void OnPause()
    {
        movespeed = 0;
    }

    public void OnUnpause()
    {
        movespeed = storedmovespeed;
    }

    public void OnDestroy()
    {
        PauseControl.TryRemovePausable(gameObject);
    }

    public void UnPausedUpdate()
    {
        Move();
    }

}