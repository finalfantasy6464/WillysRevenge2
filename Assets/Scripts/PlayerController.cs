using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Events;

public class PlayerController : MonoBehaviour
{


    public bool shieldactive = false;
    bool ate = false;
    bool Switch1Pressed = false;
    bool Switch2Pressed = false;
    public bool dirlock = false;
    public bool gotgold = false;

    public List<follower> taillist = new List<follower>();
    public GameObject tailPrefab;
    public GameObject Shield;

    // movement
    public Vector2 dir = Vector2.zero;
    public int presentdir;
    public Vector2 pushback = Vector2.zero;
    private Vector3 doormove = new Vector3(0, 0.72f, 0);
    private float moveTickTime = 0.0f;
    private Queue<Vector3> positionHistory = new Queue<Vector3>();

    // tail segments
    private float segmentDiameter;

    private float pelletno = 0.0f;
    private float pelletgap = 0.05f;
    private float moveInterval = 0.025f;

    public Sprite Sprite1;
    public Sprite Sprite2;
    public Sprite Sprite3;
    public Sprite Sprite4;

    private int count;
    private int numberofpickups;

    public AudioClip pelletget;

    public Text countText;

    public Image GoldenPellet;

    public CanvasGroup MainMenuOption;

    private AudioSource source;
    private float vollowrange = 0.8f;
    private float volhighrange = 1.0f;
    private float pitchlowrange = 0.85f;
    private float pitchhighrange = 1.15f;

    void Start()
    {
        source = GetComponent<AudioSource>();
        count = 0;
        SetCountMax();
        SetCountText();
        segmentDiameter = tailPrefab.GetComponent<SpriteRenderer>().bounds.size.x;
    }

    public void Update()
    {

        this.moveTickTime += Time.deltaTime;
        this.pelletgap += Time.deltaTime;

        if(pelletno < 0)
        {
            pelletno = 0;
        }

        if (this.moveTickTime >= moveInterval)
        {
            Move();
            moveTickTime = moveTickTime - moveInterval;
        }

        if (Input.GetKey(KeyCode.Escape))
        {
            MainMenuOption.alpha = 1;
            MainMenuOption.interactable = true;
        }

        if (Input.GetKey(KeyCode.N))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }

        if (Input.GetKey(KeyCode.B))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
        }

        if (dirlock == false)
        {
            if (Input.GetKey(KeyCode.RightArrow) || (Input.GetKey(KeyCode.D)))
            {
                presentdir = 1;
            }
            else if (Input.GetKey(KeyCode.DownArrow) || (Input.GetKey(KeyCode.S)))
            {
                presentdir = 2;
            }
            else if (Input.GetKey(KeyCode.LeftArrow) || (Input.GetKey(KeyCode.A)))
            {
                presentdir = 3;
            }
            else if (Input.GetKey(KeyCode.UpArrow) || (Input.GetKey(KeyCode.W)))
            {
                presentdir = 4;
            }
        }

        switch (presentdir)
        {

            case 4:
                dir = new Vector2(0.0f, 0.05f + pelletno) + pushback;
                this.GetComponent<SpriteRenderer>().sprite = Sprite2;
                break;
            case 3:
                dir = new Vector2(-0.05f - pelletno, 0.0f) + pushback;
                this.GetComponent<SpriteRenderer>().sprite = Sprite3;
                break;
            case 2:
                dir = new Vector2(0.0f, -0.05f - pelletno) + pushback;
                this.GetComponent<SpriteRenderer>().sprite = Sprite4;
                break;
            case 1:
                dir = new Vector2(0.05f + pelletno, 0.0f) + pushback;
                this.GetComponent<SpriteRenderer>().sprite = Sprite1;
                break;
        }

        UpdateTailSegments();
    }
    void OnTriggerEnter2D(Collider2D coll)
    {

        if (coll.gameObject.tag == "Pickup" && pelletgap >= 0.02f)
        {
            float vol = Random.Range(vollowrange, volhighrange);
            source.pitch = Random.Range(pitchlowrange, pitchhighrange);
            source.PlayOneShot(pelletget, vol);
            ate = true;
            Destroy(coll.gameObject);
            pelletno += 0.004f;
            pelletgap = 0;
            count = count + 1;
            SetCountText();
        }

        if (coll.gameObject.tag == "SuperPickup")
        {
            float vol = Random.Range(vollowrange, volhighrange);
            source.pitch = Random.Range(pitchlowrange, pitchhighrange);
            source.PlayOneShot(pelletget, vol);
            ate = true;
            Destroy(coll.gameObject);
            pelletno += 0.1f;
        }

        if (coll.gameObject.tag == "AntiPickup")
        {
            float vol = Random.Range(vollowrange, volhighrange);
            source.pitch = Random.Range(pitchlowrange, pitchhighrange);
            source.PlayOneShot(pelletget, vol);
            Destroy(coll.gameObject);
            pelletno -= 0.1f;
        }

        if (coll.gameObject.tag == "SlowPickup")
        {
            float vol = Random.Range(vollowrange, volhighrange);
            source.pitch = Random.Range(pitchlowrange, pitchhighrange);
            source.PlayOneShot(pelletget, vol);
            Destroy(coll.gameObject);
            pelletno -= 0.008f;
        }

        if (coll.gameObject.tag == "GoldenPickup")
        {
            float vol = Random.Range(vollowrange, volhighrange);
            source.pitch = Random.Range(pitchlowrange, pitchhighrange);
            source.PlayOneShot(pelletget, vol);
            Destroy(coll.gameObject);
            GoldenPellet.enabled = true;
            gotgold = true;

        }

        if (coll.gameObject.tag == "Switch")
        {

            if (Switch1Pressed == false)
            {
                GameObject[] doors = GameObject.FindGameObjectsWithTag("Oneway");

                foreach (GameObject door in doors)
                    door.transform.position = door.transform.position + doormove;

                Switch1Pressed = true;

                Switch2Pressed = false;

            }
            else
            {
                ;

            }
        }
        if (coll.gameObject.tag == "Switch2")
        {

            if (Switch2Pressed == false)
            {
                GameObject[] doors = GameObject.FindGameObjectsWithTag("Oneway");
                foreach (GameObject door in doors)
                    door.transform.position = door.transform.position - doormove;

                Switch1Pressed = false;
                Switch2Pressed = true;
            }
            else
            {
                ;
            }
        }

        if (coll.gameObject.tag == "Shield")
        {
            Vector3 s = transform.position;
            Destroy(coll.gameObject);
            GameObject shieldnew = Instantiate(Shield, s, Quaternion.identity);
            shieldnew.transform.SetParent(this.transform, true);
            shieldactive = true;
        }
    }

    void Move()
    {
        if (ate)
        {
            CreateTailSegment();
            ate = false;
        }
        transform.Translate(dir);
    }

    private void UpdateTailSegments()
    {
        foreach (follower tailSegment in taillist)
        {
            tailSegment.AddToQueue(transform.position);
            tailSegment.TryMoveNext(0.05f + pelletno);
        }
    }

    ///<Summary>
    /// Creates a new tail segment GameObject, assigns its Follower component's
    /// Transform to follow and sleepDistance, and then adds it to taillist.
    ///</Summary>
    private void CreateTailSegment()
    {
        float distanceMovedPerTick = 0.05f + pelletno;
        float distanceMovedPerSecond = distanceMovedPerTick * 40f;
        Vector3 spawnPosition = transform.position;
        GameObject segmentGameObject = Instantiate(
                tailPrefab, spawnPosition, Quaternion.identity);
        segmentGameObject.name = "Segment_Newest";
        follower segmentFollower = segmentGameObject.GetComponent<follower>();
        segmentFollower.target = transform;
        segmentFollower.speedWhenEaten = distanceMovedPerTick;
        taillist.Insert(0, segmentFollower);
        segmentGameObject.GetComponent<SpriteRenderer>().sortingOrder = 0 - taillist.Count;
        segmentFollower.sleepTime = (segmentDiameter * taillist.Count) / distanceMovedPerSecond;
        if (taillist.Count < 2) return;
        for (int i = 1; i < taillist.Count; i++)
        {
            taillist[i].gameObject.name = "Segment" + i.ToString();
            taillist[i].target = taillist[i - 1].transform;
        }
    }

    void SetCountText()
    {
        countText.text = "Pickups: " + count.ToString() + " / " + numberofpickups.ToString();
    }

    void SetCountMax()
    {
        GameObject[] totalpickups = GameObject.FindGameObjectsWithTag("Pickup");
        numberofpickups = totalpickups.Length;


    }


}