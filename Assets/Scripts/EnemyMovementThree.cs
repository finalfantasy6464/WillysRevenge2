using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemyMovementThree : MonoBehaviour, IPausable
{
	public int waypointIndex = 0;
	public float movespeed = 2f;
	public Transform[] waypoints;

	public bool stopsonRoute = false;
	public float stopcounter;
	public float stoptime = 0.5f;
	ParticleSystem particles;

    public bool isPaused { get; set; }

    void Start(){
		transform.position = waypoints[waypointIndex].transform.position;
		particles = GetComponent<ParticleSystem>();
	}

     void Update(){
        if (!isPaused)
        {
			UnPausedUpdate();
        }
	}

	void Move() {
        if (!stopsonRoute)
        {
			transform.position = Vector3.MoveTowards(transform.position, waypoints[waypointIndex].transform.position, movespeed * Time.smoothDeltaTime);

			if (transform.position == waypoints[waypointIndex].transform.position)
			{
				waypointIndex += 1;
			}

			if (waypointIndex >= waypoints.Length)
				waypointIndex = 0;
		}

		if(stopsonRoute)
		{
			stopcounter += Time.deltaTime;

			if(stopcounter >= stoptime)
			transform.position = Vector3.MoveTowards(transform.position, waypoints[waypointIndex].transform.position, movespeed * Time.smoothDeltaTime);

			if (transform.position == waypoints[waypointIndex].transform.position)
			{
				waypointIndex += 1;
				stopcounter = 0;
			}

			if (waypointIndex >= waypoints.Length)
				waypointIndex = 0;
		}
	}

    public void OnPause()
    {
		if(particles != null)
        {
			particles.Pause();
        }
	}

    public void OnUnpause()
    {
		if (particles != null)
		{
			particles.Play();
		}
	}

    public void UnPausedUpdate()
    {
		Move();
	}

	public void OnDestroy()
    {
        PauseControl.TryRemovePausable(gameObject);
    }
}