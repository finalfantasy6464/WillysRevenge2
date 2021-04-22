using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleporterThree : MonoBehaviour{

	public Transform target3;

    private bool Justhit;

    private void Update()
    {
        Justhit = false;
    }

    void OnTriggerEnter2D(Collider2D tele)
{

		var hit = tele.gameObject;

		if (hit.tag == "Player"){
			hit.transform.position = target3.position;
		}


		if (hit.tag == "Enemy2" || hit.tag == "Enemy3"){
			hit.transform.position = target3.position;
            if (Justhit == false)
            {
                hit.GetComponent<EnemyMovementThree>().waypointIndex += 1;
                Justhit = true;
            }
   
            }
             
		}

	}
