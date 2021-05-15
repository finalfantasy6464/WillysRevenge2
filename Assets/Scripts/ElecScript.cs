using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.SceneManagement;
using UnityEngine.Events;

public class ElecScript : MonoBehaviour{

	public Sprite[] elecSprites;

    private float electimer = 0.0f;
	public int CurrentSprite = 1;
	public int mysprite = 0;

	public AudioClip shock;

    [HideInInspector] public UnityEvent onShockEvent;

    void Start()
    {
        onShockEvent = new UnityEvent();
    }
    void Update(){

		var renderer = GetComponent<SpriteRenderer> ();
		renderer.sprite = elecSprites [mysprite];
		this.electimer += Time.deltaTime;

		if (electimer >= 0.05f) {
			CurrentSprite += 1;
			electimer = 0.0f;
			mysprite++;

			if (CurrentSprite == 18) {
				CurrentSprite = 0;
			}

			if (mysprite == elecSprites.Length) {
				mysprite = 0;
			}
		}
	}
		
	void OnTriggerStay2D (Collider2D Elec){

		var mysprite = this.GetComponent<SpriteRenderer> ().sprite;
		var hit = Elec.gameObject;
			
		if (hit.tag == "Player" & mysprite == elecSprites [9]){
			Destroy (hit);
            onShockEvent.Invoke();
			SceneManager.LoadScene (SceneManager.GetActiveScene ().name);
}
}
}