using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShiftScriptTwo : MonoBehaviour
{
	private PlayerController playercontroller;
	GameObject player;
	public int sanddirection;
    public float pushstrengthHoriz = 0.1f;
    public float pushstrengthVert = 0.1f;

    void Start(){
		player = GameObject.Find ("Player");
		playercontroller = player.GetComponent<PlayerController> ();
	}
			
	void OnCollisionStay2D(Collision2D sand){
		
		var hit = sand.gameObject;

			if (hit.tag == "Player"){

			switch (sanddirection) {

			case 4:
				switch (playercontroller.presentdir) {

				case 4:
					playercontroller.pushback = new Vector2 (0.0f, pushstrengthVert);
					break;
				case 3:
					playercontroller.pushback = new Vector2 (0.0f, pushstrengthVert / 10);
					break;
				case 2:
					playercontroller.pushback = new Vector2 (0.0f, pushstrengthVert);
					break;
				case 1:
					playercontroller.pushback = new Vector2 (0.0f, pushstrengthVert / 10);
					break;
				}
				break;
			case 3:
				switch (playercontroller.presentdir) {

				case 4:
					playercontroller.pushback = new Vector2 (-pushstrengthHoriz / 10, 0.0f);
					break;
				case 3:
					playercontroller.pushback = new Vector2 (-pushstrengthHoriz, 0.0f);
					break;
				case 2:
					playercontroller.pushback = new Vector2 (-pushstrengthHoriz / 10, 0.0f);
					break;
				case 1:
					playercontroller.pushback = new Vector2 (-pushstrengthHoriz, 0.0f);
					break;
				}
				break;
			case 2:
				switch (playercontroller.presentdir) {

				case 4:
					playercontroller.pushback = new Vector2 (0.0f, -pushstrengthVert);
					break;
				case 3:
					playercontroller.pushback = new Vector2 (0.0f, -pushstrengthVert / 10);
					break;
				case 2:
					playercontroller.pushback = new Vector2 (0.0f, -pushstrengthVert);
					break;
				case 1:
					playercontroller.pushback = new Vector2 (0.0f, -pushstrengthVert / 10);
					break;
				}
				break;
			case 1:
				switch (playercontroller.presentdir) {

				case 4:
					playercontroller.pushback = new Vector2 (pushstrengthHoriz / 10, 0.0f);
					break;
				case 3:
					playercontroller.pushback = new Vector2 (pushstrengthHoriz, 0.0f);
					break;
				case 2:
					playercontroller.pushback = new Vector2 (pushstrengthHoriz / 10, 0.0f);
					break;
				case 1:
					playercontroller.pushback = new Vector2 (pushstrengthHoriz, 0.0f);
					break;	
				}
				break;
}
}
	}
	void OnCollisionExit2D(Collision2D sand){

		var hit = sand.gameObject;
		if (hit.tag == "Player"){
			playercontroller.pushback = Vector2.zero;
		}
}
}