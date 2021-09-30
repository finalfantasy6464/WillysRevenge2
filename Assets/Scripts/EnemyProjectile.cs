﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectile : MonoBehaviour, IPausable
{

    public GameObject bullet;

    public float fireRate;
    private float nextFire;
    public float shotspeed;

    Transform player;
    Vector2 firingangle;

    public bool isPaused { get; set; }

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }
    // Update is called once per frame
    void Update()
    {
        if (!isPaused)
        {
            UnPausedUpdate();
        }
    }

	void CheckIfTimeToFire()
	{
        nextFire += Time.deltaTime;

		if (nextFire >= fireRate)
        {
            nextFire -= nextFire;
			GameObject newBullet = Instantiate (bullet, transform.position, Quaternion.identity);
            if (newBullet.TryGetComponent(out Rigidbody2D bulletBody))
            {
                firingangle = player.position - transform.position;
                newBullet.GetComponent<Aimedbullet>().SetForce(firingangle.normalized * 100f);
                bulletBody.AddForce(firingangle.normalized * 100f);
            }
            PauseControl.TryAddPausable(newBullet);
	}
}

    public void OnPause()
    { }

    public void OnUnpause()
    { }

    public void PausedUpdate()
    { }

    public void UnPausedUpdate()
    {
        CheckIfTimeToFire();
    }

    public void OnDestroy()
    {
        PauseControl.TryRemovePausable(gameObject);
    }
}