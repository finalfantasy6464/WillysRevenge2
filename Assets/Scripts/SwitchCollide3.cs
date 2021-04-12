using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchCollide3 : MonoBehaviour
{

	public bool Active;

	public Sprite changesprite;

	void OnTriggerEnter2D(Collider2D coll){
		if (coll.gameObject.tag == "Player") {
			Active = true;
			this.GetComponent<SpriteRenderer> ().sprite = changesprite;
		}
	}
}
