using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SawbladeArray : MonoBehaviour, IPausable
{

	public float factor = 1.0f;
	public float amplitude;
	public float speed = 1.0f;
	public float StartSpawnX;
	public float StartSpawnY;
	public float nextSawXoffset = 0.5f;
	public float nextSawYoffset = 0.5f;
	private float spintimer;
	public int Direction = 1;
	public int sawnumber = 0;
	public GameObject[] waveSaw;
	public GameObject Saw;

	public bool isPaused { get; set; }

	public void OnPause()
	{ }

	public void OnUnpause()
	{ }
	
	void Start()
	{
		waveSaw = new GameObject[sawnumber];

		switch (Direction)
		{

			case 2:
				for (int i = 0; i < sawnumber; i++)
				{
					waveSaw[i] = Instantiate(Saw, new Vector3(0.0f, i - StartSpawnY + (i * nextSawYoffset), -0.10f), Quaternion.identity) as GameObject;
				}
				break;

			case 1:
				for (int i = 0; i < sawnumber; i++)
				{
					waveSaw[i] = Instantiate(Saw, new Vector3(i - StartSpawnX + (i * nextSawXoffset), 0.0f, -0.10f), Quaternion.identity) as GameObject;
				}
				break;
		}
	}
	// Update is called once per frame
	void Update()
	{
        if (!isPaused)
        {
			UnPausedUpdate();
        }
	}


	public void UnPausedUpdate()
	{
		spintimer += Time.deltaTime;

		switch (Direction)
		{

			case 2:
				for (int i = 0; i < sawnumber; i++)
				{

					waveSaw[i].transform.Rotate(0, 0, 4);
					Vector3 position = waveSaw[i].transform.position;
					position.x = StartSpawnX + Mathf.Sin((spintimer * speed) + i * factor) * amplitude;
					waveSaw[i].transform.position = position;
				}
				break;
			case 1:
				for (int i = 0; i < sawnumber; i++)
				{

					waveSaw[i].transform.Rotate(0, 0, 4);
					Vector3 position = waveSaw[i].transform.position;
					position.y = StartSpawnY + Mathf.Sin((spintimer * speed) + i * factor) * amplitude;
					waveSaw[i].transform.position = position;
				}
				break;
		}
	}

	public void OnDestroy()
    {
        PauseControl.TryRemovePausable(gameObject);
    }
}

