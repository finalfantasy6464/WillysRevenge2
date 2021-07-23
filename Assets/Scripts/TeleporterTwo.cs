using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleporterTwo : MonoBehaviour{

	public Transform target2;

    public PositionalSFX sfx;
    public PositionalSFX sfx2;

    // Update is called once per frame

    void OnTriggerEnter2D(Collider2D tele)
{

		var hit = tele.gameObject;

		if (hit.tag == "Player"){
			hit.transform.position = target2.position;
            sfx.PlayPositionalSound();
            sfx2.PlayPositionalSound();
        }


		if (hit.tag == "Enemy2" || hit.tag == "Enemy3"){
			hit.transform.position = target2.position;
            sfx.PlayPositionalSound();
            sfx2.PlayPositionalSound();
        }
	}
}