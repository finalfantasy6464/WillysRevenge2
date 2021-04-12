using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchCollide : MonoBehaviour
{

	public Sprite defaultsprite;
	public Sprite changesprite;

	void OnTriggerEnter2D(Collider2D coll) {
		if (coll.gameObject.tag == "Player") {
			this.GetComponent<SpriteRenderer>().sprite = changesprite;
		}
	}
}