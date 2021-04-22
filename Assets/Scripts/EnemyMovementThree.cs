using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemyMovementThree : MonoBehaviour
{
	public int waypointIndex = 0;
	public float movespeed = 2f;
	public Transform[] waypoints;

	void Start(){
		transform.position = waypoints[waypointIndex].transform.position;
	}

     void Update(){
		Move ();
	}

	void Move() {
		transform.position = Vector3.MoveTowards (transform.position, waypoints [waypointIndex].transform.position, movespeed * Time.smoothDeltaTime);

		if (transform.position == waypoints [waypointIndex].transform.position) {
			waypointIndex += 1;
		}

		if (waypointIndex >= waypoints.Length)
			waypointIndex = 0;
	}
}