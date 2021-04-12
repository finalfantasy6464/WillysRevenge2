using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingShooterArrow : MonoBehaviour
{

	public bool Active;
	private Vector3 Distance;
	private float DistanceFrom;
	public float firerate = 0.5f;
	private float nextfire = 0.0f;
	public float offset = 0.0f;

	public Transform Spawnpoint;

	public GameObject ammo;
	public float shotspeed = 100.0f;


	void Start(){
		nextfire = offset;
	}

    void Update()
	{

		nextfire += Time.deltaTime;

		if (Active) {
			Attacking ();
		}
	}

	void Attacking(){

		if (nextfire >= firerate) {
			nextfire = 0.0f;

				GameObject Shoot = Instantiate (ammo, Spawnpoint.position, Spawnpoint.rotation) as GameObject;
				Shoot.GetComponent<Rigidbody2D> ().AddForce (Spawnpoint.right * shotspeed);

			}
	}
}