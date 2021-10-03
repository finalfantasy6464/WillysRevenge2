using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemyMovementFour : MonoBehaviour, IPausable
{
	int waypointIndex = 0;
	public int flipnumber = 0;
	public float movespeed = 2f;
	public float rotatespeed = 8f;
	public Transform[] waypoints;

	Collider2D m_Collider;
	public Sprite defaultsprite;
	SpriteRenderer sprite;

    public bool isPaused { get; set; }

    void Start(){
		transform.position = waypoints[waypointIndex].transform.position;
		m_Collider = GetComponent<Collider2D>();
		sprite = GetComponent<SpriteRenderer> ();

	}

     void Update(){
        if (!isPaused)
        {
			UnPausedUpdate();
        }
	}

	void Move() {
		transform.position = Vector3.MoveTowards (transform.position, waypoints [waypointIndex].transform.position, movespeed * Time.deltaTime);
		transform.rotation = Quaternion.Lerp (transform.rotation, waypoints [waypointIndex].transform.rotation, rotatespeed * Time.deltaTime);

		if (transform.position == waypoints [waypointIndex].transform.position) {
			waypointIndex += 1;
		}

		if (waypointIndex == waypoints.Length)
			waypointIndex = 0;

		if (waypointIndex == flipnumber) {
			m_Collider.enabled = true;
			sprite.sprite = defaultsprite;
			sprite.sortingOrder = 0;
		}
	}

	public void OnPause()
	{}

	public void OnUnpause()
	{}

    public void PausedUpdate()
    {}

    public void UnPausedUpdate()
    {
		Move();
	}

	public void OnDestroy()
    {
        PauseControl.TryRemovePausable(gameObject);
    }
}