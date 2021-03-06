using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ArenaCollision : MonoBehaviour {
 
	public AudioClip wallhit;

	void OnCollisionEnter2D(Collision2D col){

		var hit = col.gameObject;

		if(hit.tag == "Player"){

			string name = "Player";
			GameObject player = GameObject.Find (name);
			Destroy (player.gameObject);
			GameSoundManagement.instance.PlaySingle (wallhit);
			SceneManager.LoadScene (SceneManager.GetActiveScene().name);

}
}
}
