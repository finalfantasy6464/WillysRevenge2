using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemyMovementThreeVariant : MonoBehaviour, IPausable
{
	public int waypointIndex = 0;
	public float movespeed = 2f;
    public float storedmovespeed;
	public Transform[] waypoints;
    bool finished;

    Animator m_Animator;

    public bool isPaused { get; set; }

    void Start()
    {
		transform.position = waypoints[waypointIndex].transform.position;
        storedmovespeed = movespeed;
        m_Animator = GetComponent<Animator>();
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
        {
            finished = true;
            m_Animator.Play("Enemy3Suction");
        }
    }

    void DestroySelf()
    {
        Destroy(this.gameObject);
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
        if(!finished)
        Move();
    }

}