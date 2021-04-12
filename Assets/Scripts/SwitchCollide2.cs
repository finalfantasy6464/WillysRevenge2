using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchCollide2 : MonoBehaviour
{

	private SwitchCollide switchcollide;
	GameObject Switch;

	void Start(){
		Switch = GameObject.FindGameObjectWithTag ("Switch");
		switchcollide = Switch.GetComponent<SwitchCollide> ();
	}

	void OnTriggerEnter2D(Collider2D coll) {
		
		if (coll.gameObject.tag == "Player") {
			switchcollide.GetComponent<SpriteRenderer>().sprite = switchcollide.defaultsprite;
		}
	}
		}