using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingShooterArrow : MonoBehaviour, IPausable
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

    public bool isPaused { get; set; }

    void Start(){
		nextfire = offset;
	}

    void Update()
	{
        if (!isPaused)
			UnPausedUpdate();
	}

	void Attacking(){

		if (nextfire >= firerate) {
			nextfire -= nextfire;

			GameObject Shoot = Instantiate (ammo, Spawnpoint.position, Spawnpoint.rotation) as GameObject;
			if (Shoot.TryGetComponent(out Rigidbody2D bulletBody))
			{
				bulletBody.AddForce(Spawnpoint.right * shotspeed);
				Shoot.GetComponent<Bullet>().SetForce(Spawnpoint.right * shotspeed);
			}
			PauseControl.TryAddPausable(Shoot);

		}
	}

	public void OnDestroy()
	{
		PauseControl.TryRemovePausable(gameObject);
	}

	public void OnPause()
	{ }

	public void OnUnpause()
	{ }

    public void UnPausedUpdate()
    {
		nextfire += Time.deltaTime;

		if (Active)
		{
			Attacking();
		}
	}
}