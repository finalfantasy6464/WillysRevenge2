using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ExitCollision : MonoBehaviour {
 
	public int currentlevelID;

	Scene m_Scene;
	public string sceneName;
	public int nocomplete;

	PlayerController playercontroller;
	GameObject player;

	LevelTimer leveltimer;
	GameObject timetracker;

	void Start(){
		player = GameObject.FindGameObjectWithTag ("Player");
		timetracker = GameObject.Find ("Timer");
		playercontroller = player.GetComponent<PlayerController> ();
		leveltimer = timetracker.GetComponent<LevelTimer> ();

		m_Scene = SceneManager.GetActiveScene ();
		sceneName = m_Scene.name;
		currentlevelID = int.Parse(sceneName.Substring(5));
	}

	void OnCollisionEnter2D(Collision2D col){


		if (GameControl.control.returntoselect == true) {
			GameControl.control.returntoselect = false;
			SceneManager.LoadScene ("MainMenu");
		} else {
		var hit = col.gameObject;
			if (hit.tag == "Player") {
				SceneManager.LoadScene ("Overworld");
			if (GameControl.control.completedlevels[currentlevelID] == false) {
				GameControl.control.completedlevels[currentlevelID] = true;
				GameControl.control.complete += 1;

			}
			if (GameControl.control.goldenpellets[currentlevelID] == false && playercontroller.gotgold == true) {
				GameControl.control.goldenpellets[currentlevelID] = true;
				GameControl.control.golden += 1;
			}

			if (GameControl.control.timerchallenge [currentlevelID] == false && leveltimer.expired == false)
				GameControl.control.timerchallenge [currentlevelID] = true;
			    GameControl.control.timer += 1;
	                                 }
}
	}
}