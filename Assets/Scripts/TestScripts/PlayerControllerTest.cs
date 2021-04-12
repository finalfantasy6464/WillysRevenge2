using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.SceneManagement;

public class PlayerControllerTest : MonoBehaviour {


	bool ate = false;
    bool Switch1Pressed = false;
	bool Switch2Pressed = false;
	public bool dirlock = false;

	public List<Transform> taillist = new List<Transform> ();
	public GameObject tailPrefab;

	public Vector3 dir = new Vector3 (0,0,-0.01f);
	public Vector3 pushback = Vector3.zero;
	private Vector3 doormove = new Vector3 (0,0.72f,0);
	public float xoffset = 0.0f;
	public float yoffset = 0.0f;
	private float movespeed = 0.0f;
	private float pelletno = 0.0f;
	public Sprite Sprite1;
	public Sprite Sprite2;
	public Sprite Sprite3;
	public Sprite Sprite4;
	public int presentdir;


	void Start(){


	}

	public void Update(){
		
		this.movespeed += Time.deltaTime;

		if (this.movespeed >= 0.025f) {
			Move ();
			movespeed = 0.0f;
		}

		if (Input.GetKey (KeyCode.N)){
			SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
		}
			
		if (dirlock == false) {
			if (Input.GetKey (KeyCode.RightArrow) || (Input.GetKey (KeyCode.D))) {
				presentdir = 1;
			} else if (Input.GetKey (KeyCode.DownArrow) || (Input.GetKey (KeyCode.S))) {
				presentdir = 2;
			} else if (Input.GetKey (KeyCode.LeftArrow) || (Input.GetKey (KeyCode.A))) {
				presentdir = 3;
			} else if (Input.GetKey (KeyCode.UpArrow) || (Input.GetKey (KeyCode.W))) {
				presentdir = 4;

			} else {
			}
		}

		switch (presentdir) {

		case 4:
			dir = new Vector3 (0.0f, 0.05f + pelletno, 0.0f) + pushback ;
			this.GetComponent<SpriteRenderer> ().sprite = Sprite2;
			break;
		case 3:
			dir = new Vector3 (-0.05f - pelletno, 0.0f, 0.0f) + pushback;
		this.GetComponent<SpriteRenderer> ().sprite = Sprite3;
			break;
		case 2:
			dir = new Vector3 (0.0f, -0.05f - pelletno, 0.0f) + pushback;
			this.GetComponent<SpriteRenderer> ().sprite = Sprite4;
			break;
		case 1:
			dir = new Vector3 (0.05f + pelletno, 0.0f, 0.0f) + pushback;
			this.GetComponent<SpriteRenderer> ().sprite = Sprite1;
			break;
		}
	}
	void OnTriggerEnter2D (Collider2D coll){
		
		if (coll.gameObject.tag == "Pickup") {
			ate = true;
			Destroy (coll.gameObject);
			pelletno += 0.005f;
		}

		if (coll.gameObject.tag == "SuperPickup") {
			ate = true;
			Destroy (coll.gameObject);
			pelletno += 0.1f;
		}

		if (coll.gameObject.tag == "AntiPickup") {
			Destroy (coll.gameObject);
			pelletno -= 0.1f;
		}

		if (coll.gameObject.tag == "Switch") {

			if (Switch1Pressed == false) {
				GameObject[] doors = GameObject.FindGameObjectsWithTag ("Oneway");

				foreach (GameObject door in doors)
					door.transform.position = door.transform.position + doormove;

				Switch1Pressed = true;
			
				Switch2Pressed = false;
			
			} else {
				;

			}
		}
		if (coll.gameObject.tag == "Switch2") {

				if (Switch2Pressed == false) {
					GameObject[] doors = GameObject.FindGameObjectsWithTag ("Oneway");
					foreach (GameObject door in doors)
						door.transform.position = door.transform.position - doormove;

					Switch1Pressed = false;
					Switch2Pressed = true;
				} else {
					;
				}
			

		}
	}
    void Move()
    {

        Vector3 v = transform.position;
        transform.Translate(dir);
    }
}
