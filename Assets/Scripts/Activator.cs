using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class Activator : MonoBehaviour
{
	private EnemyMovementThree pathing;


	void Start (){
		pathing = GetComponent<EnemyMovementThree> ();
	}

	void OnTriggerEnter2D(Collider2D trig){
		var hit = trig.gameObject;

			if (hit.tag == "Player" && hit.tag != null){
            if (pathing != null)
            {
             pathing.enabled = true;
            }
				
			}
	}
}

