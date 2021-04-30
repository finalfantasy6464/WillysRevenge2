using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemyMovementTwo : MonoBehaviour
{

	private float movespeed = 0.0f;
    private float moveinterval = 0.1f;

	public float multiplier = 1.0f;

	private Vector2 enemydir = Vector2.zero;

    public bool lifespan = false;
	private bool justhit = false;
    private bool levelstart = false;

	public int direction = 1;

    private int hitcount;


    private float age = 0;
    public float death;

    void Start()
    {
        StartCoroutine(LevelStarting());
    }

    IEnumerator LevelStarting()
    {
        if (levelstart == false)
        {
            yield return new WaitForSeconds(0.2f);
            levelstart = true;
        }
    }

    void OnCollisionEnter2D(Collision2D coll)
    {

        while (direction == 1)
        {

            if (justhit == true)
            {
                break;
            }
            else
            {
                if (coll.gameObject.tag == "Enemy2")
                {
                    hitcount += 1;
                    direction = 2;
                    justhit = true;
                }

                if (coll.gameObject.tag != "Teleport2" && coll.gameObject.tag != "Enemy2")
                {
                
                    direction = 2;
                    justhit = true;
                }
                if (coll.gameObject.tag == "Teleport2")
                {

                    justhit = true;
                }
            }
        }

        while (direction == 2)
        {

            if (justhit == true)
            {
                break;
            }
            else
            {

                if (coll.gameObject.tag == "Enemy2")
                {
                    hitcount += 1;
                    direction = 1;
                    justhit = true;
                }

                if (coll.gameObject.tag != "Teleport2" && coll.gameObject.tag != "Enemy2")
                {

                    direction = 1;
                    justhit = true;
                }
                if (coll.gameObject.tag == "Teleport2")
                {

                    justhit = true;
                }
            }
        }
    }

     void Update(){

        if(lifespan == true)
        {
            age += Time.smoothDeltaTime;
            if(age >= death)
            {
                Destroy(gameObject);
            }
        }

        if(levelstart == true)
        {
         this.movespeed += Time.deltaTime * multiplier;
        }
		
		if (this.movespeed >= moveinterval) {
			Move ();
			movespeed = movespeed - moveinterval;
		}

        if(hitcount >= 10)
        {
            Destroy(gameObject);
        }

		switch (direction) {

		case 2:
			enemydir = Vector2.up;
			break;
				
		case 1:
			
			enemydir = Vector2.down;
			break;
		}
	}

	void LateUpdate(){
		justhit = false;
	}

	void Move() {

		Vector3 v = transform.position;

		transform.Translate (enemydir * movespeed);
	}
}