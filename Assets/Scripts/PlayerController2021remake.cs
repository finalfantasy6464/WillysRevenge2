using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Events;

public class PlayerController2021remake : MonoBehaviour
{
    //Otherobjects
    public bool shieldactive = false;
    public GameObject Shield;
    bool Switch1Pressed = false;
    bool Switch2Pressed = false;
    private Vector3 doormove = new Vector3(0, 0.72f, 0);

    public bool dirlock = false;
    public bool gotgold = false;
 
    private float pelletgap;
    Rigidbody2D rb;

    SpriteRenderer spriteRenderer;

    //Segments
    public List<GameObject> taillist = new List<GameObject>();
    private List<Vector3> PositionHistory = new List<Vector3>();
    private List<Vector3> ScaleHistory = new List<Vector3>();
    public List<float> AlphaHistory = new List<float>();
    public int followDelay;
    public bool debugShowFromStart;
    public Sprite[] SegmentSprites;
    public int offsetFromHead;
    public int LatestSegmentDelay;
    public int SegmentDelaySleep = 5;
    public int SegmentDelaySleepcache;
    public int SegmentDelaySleepCounter;
    public int SegmentHistorySkipCounter;
    public int SegmentHistorySkipTimer;
    public bool ate;

    // movement
    public int presentdir;
    public Vector3 direction = Vector3.zero;
    public Vector3 FinalMovementVector = Vector3.zero;
    public Vector3 shiftVector = Vector3.zero;

    private bool moving;
    private float rotationAngle;
    public bool canmove = true;
    public bool directionreset = false;

    public float basespeed = 1.8f;
    public float finalspeed;
    private float pelletmagnitude = 0.08f;
    private float horizontal;
    private float vertical;

    public float pelletno = 0.0f;

    private int Scorecount;

    private int numberofpickups;

    public Text countText;

    public Image GoldenPellet;

    public CanvasGroup MainMenuOption;

    [HideInInspector] public UnityEvent onEatPellet;
    [HideInInspector] public UnityEvent onGoldenPellet;
    [HideInInspector] public UnityEvent onSlidingDoorOpen;
    [HideInInspector] public UnityEvent onSlidingDoorClose;
    [HideInInspector] public UnityEvent onCollectShield;

    void Awake()
    {
        onEatPellet = new UnityEvent();
        onGoldenPellet = new UnityEvent();
        onSlidingDoorOpen = new UnityEvent();
        onSlidingDoorClose = new UnityEvent();
        onCollectShield = new UnityEvent();

        SegmentDelaySleepcache = SegmentDelaySleep;
    }

    void Start()
    {
        Scorecount = 0;
        dirlock = false;
        SetCountMax();
        SetCountText();
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        if (debugShowFromStart == true)
        {
            foreach (GameObject segment in taillist)
            {
                segment.SetActive(true);
            }
        }
    }

    void Update()
    {
        Move();
        UpdateSegmentsPosition();
        UpdateSegmentDelay();
        UpdateSegmentScale();
        UpdateSegmentAlpha();

        PauseOptionsCheck();
        DebugOptionsCheck();

        PositionHistory.Insert(0, transform.position);
        ScaleHistory.Insert(0, transform.localScale);
        AlphaHistory.Insert(0, spriteRenderer.color.a);

        if(PositionHistory.Count > 1000)
        {
            PositionHistory.RemoveRange(1000, PositionHistory.Count - 1000);
        }

        if (ScaleHistory.Count > 1000)
        {
            ScaleHistory.RemoveRange(1000, ScaleHistory.Count - 1000);
        }

        if (AlphaHistory.Count > 1000)
        {
            AlphaHistory.RemoveRange(1000, AlphaHistory.Count - 1000);
        }

        this.pelletgap += Time.deltaTime;
    }

    void Move()
    {
        horizontal = Input.GetAxisRaw("Horizontal");
        vertical = Input.GetAxisRaw("Vertical");

        //Vector3 direction = Vector3.zero;
       if(vertical != 0 && dirlock == false || horizontal != 0 && dirlock == false)
        {
            if (horizontal > 0)
            {
                direction.x = 1;
                direction.y = 0;
                rotationAngle = 0;
            }
            else if (horizontal < 0)
            {
                direction.x = -1;
                direction.y = 0;
                rotationAngle = 180;
            }
            else if (vertical > 0)
            {
                direction.y = 1;
                direction.x = 0;
                rotationAngle = 90;
            }
            else if (vertical < 0)
            {
                direction.y = -1;
                direction.x = 0;
                rotationAngle = 270;
            }
            transform.rotation = Quaternion.Euler(0, 0, rotationAngle);
        }

        finalspeed = basespeed + (pelletmagnitude * pelletno);

        if (canmove)
        {
            FinalMovementVector = shiftVector + (direction * finalspeed) * Time.deltaTime;
            transform.position += FinalMovementVector;
        }
        else
        {
            direction.x = 0;
            direction.y = 0;
            FinalMovementVector = Vector3.zero;
            transform.Translate(FinalMovementVector);
        }
    }

    void HistorySkipCheck(int speedIncrease)
    {
        //Need to figure out what the ? should be.

        //PositionHistory.RemoveRange(0, speedIncrease * ?);
        //ScaleHistory.RemoveRange(0, speedIncrease * ?);
        //AlphaHistory.RemoveRange(0, speedIncrease * ?);
    }

    void UpdateSegmentOrder(Sprite segmentsprite)
    {
        SpriteRenderer currentrenderer;

        for (int i = 0; i < taillist.Count; i++)
        {
            currentrenderer = taillist[i].GetComponent<SpriteRenderer>();
            currentrenderer.sortingOrder = -i;

            if (!taillist[i].activeInHierarchy)
            {
                break;
            }
        }

        SpriteRenderer previousrenderer;

        for (int i = taillist.Count - 1; i > 0; i--)
        {
            if (!taillist[i].activeInHierarchy)
            {
                continue;
            }
            currentrenderer = taillist[i].GetComponent<SpriteRenderer>();
            previousrenderer = taillist[i - 1].GetComponent<SpriteRenderer>();
            currentrenderer.sprite = previousrenderer.sprite;
        }

        taillist[0].GetComponent<SpriteRenderer>().sprite = segmentsprite;
    }

    void UpdateSegmentTint()
    {
        SpriteRenderer currentrenderer;
        
        for (int i = 0; i < taillist.Count; i++)
        {
            currentrenderer = taillist[i].GetComponent<SpriteRenderer>();
            currentrenderer.color = new Color(1f - (i * 0.007f), 1f - (i * 0.007f), 1f - (i * 0.007f), 1f);
           
            if (!taillist[i].activeInHierarchy)
            {
                break;
            }
        }
    }

    void UpdateSegmentAlpha()
    {
        SpriteRenderer currentrenderer;

        for (int i = 0; i < taillist.Count; i++)
        {
            if (!taillist[i].activeInHierarchy)
            {
                break;
            }

            currentrenderer = taillist[i].GetComponent<SpriteRenderer>();
            if (AlphaHistory.Count > offsetFromHead + (followDelay * i))
            {
                currentrenderer.color = new Color(currentrenderer.color.r, currentrenderer.color.b, currentrenderer.color.g, AlphaHistory[offsetFromHead + (followDelay * i)]);
            }
        }
    }

    void ShowNextSegment(Sprite segmentsprite, int speedIncrease)
    {
        SegmentHistorySkipCounter = offsetFromHead;

        if (ate)
        {
            LatestSegmentDelay = 1;
            SegmentDelaySleepCounter = SegmentDelaySleep;
        }
        ate = true;
        LatestSegmentDelay = offsetFromHead;
        SegmentDelaySleep = SegmentDelaySleepcache;

        for (int i = 0; i < taillist.Count; i++)
        {
            if (!taillist[i].activeInHierarchy)
            {
                taillist[i].SetActive(true);
                break;
            }
        }
        UpdateSegmentOrder(segmentsprite);
        UpdateSegmentTint();
        HistorySkipCheck(speedIncrease);
    }

    void UpdateSegmentsPosition()
    {
        int delay;

        for (int i = 0; i < taillist.Count; i++)
        {
            if (!taillist[i].activeInHierarchy)
            {
                break;
            }
            delay = offsetFromHead + (followDelay * i);
            
            
            taillist[i].transform.rotation = Quaternion.Euler(0, 0, 0);
            if (PositionHistory.Count > delay)
            {
                taillist[i].transform.position = PositionHistory[delay];
            }
        }
    }

    void UpdateSegmentDelay()
    {
        if (!ate)
        {
          return;
        }

        if (SegmentDelaySleepCounter < SegmentDelaySleep)
        {
            SegmentDelaySleepCounter++;
        }
        else
        {
            LatestSegmentDelay--;
            if (LatestSegmentDelay == 0)
            {
                ate = false;
                LatestSegmentDelay = offsetFromHead;
                SegmentDelaySleep = SegmentDelaySleepcache;
            }
            else
            {
                SegmentDelaySleep = Mathf.Max(0, SegmentDelaySleep - 1);
            }
            SegmentDelaySleepCounter = 0;
        }
    }

    void UpdateSegmentScale()
    {
        Vector3 ScaleFactor;
        for (int i = 0; i < taillist.Count; i++)
        {
            if (!taillist[i].activeInHierarchy)
            {
                break;
            }

            if (ScaleHistory.Count > offsetFromHead + (followDelay * i))
            {
                ScaleFactor = ScaleHistory[offsetFromHead + (followDelay * i)];
                taillist[i].transform.localScale = new Vector3(ScaleFactor.x / transform.localScale.x, ScaleFactor.y / transform.localScale.y, 1); 
            }
        }
    }

    void PauseOptionsCheck()
    {
        if (Input.GetKey(KeyCode.Escape))
        {
            MainMenuOption.alpha = 1;
            MainMenuOption.interactable = true;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Pickup"))
        {
            onEatPellet.Invoke();
            Destroy(other.gameObject);
            pelletgap = 0;
            pelletno += 1;
            Scorecount = Scorecount + 1;
            SetCountText();
            if (!debugShowFromStart)
            {
                ShowNextSegment(SegmentSprites[0], 1);
            }
        }

        if (other.gameObject.CompareTag("GoldenPickup"))
        {
            onGoldenPellet.Invoke();
            Destroy(other.gameObject);
            GoldenPellet.enabled = true;
            gotgold = true;
            if (!debugShowFromStart)
            {
                ShowNextSegment(SegmentSprites[1], 0);
            }
        }

        if (other.gameObject.tag == "SuperPickup")
        {
            onEatPellet.Invoke();
            Destroy(other.gameObject);
            pelletno += 40;
            if (!debugShowFromStart)
            {
               ShowNextSegment(SegmentSprites[4], 40);
            }
        }

        if (other.gameObject.CompareTag("AntiPickup"))
        {
            onEatPellet.Invoke();
            Destroy(other.gameObject);
            pelletno = Mathf.Max(0, pelletno - 40);
            if (!debugShowFromStart)
            {
                ShowNextSegment(SegmentSprites[2], 0);
            }
        }

        if (other.gameObject.CompareTag("SlowPickup"))
        {
            onEatPellet.Invoke();
            Destroy(other.gameObject);
            pelletno = Mathf.Max(0, pelletno - 5);
            if (!debugShowFromStart)
            {
                ShowNextSegment(SegmentSprites[3], 0);
            }
        }

        if (other.gameObject.CompareTag("Switch"))
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
        if (other.gameObject.CompareTag("Switch2"))
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

        if (other.gameObject.CompareTag("Shield"))
        {
            onCollectShield.Invoke();
            Vector3 s = transform.position;
            Destroy(other.gameObject);
            GameObject shieldnew = Instantiate(Shield, s, Quaternion.identity);
            shieldnew.transform.SetParent(this.transform, true);
            shieldactive = true;
        }
}

    void SetCountText()
    {
        countText.text = "Pickups: " + Scorecount.ToString() + " / " + numberofpickups.ToString();
    }

    void SetCountMax()
    {
        GameObject[] totalpickups = GameObject.FindGameObjectsWithTag("Pickup");
        numberofpickups = totalpickups.Length;
    }

    void DebugOptionsCheck()
    {
        if (Input.GetKey(KeyCode.N))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }

        if (Input.GetKey(KeyCode.B))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
        }
    }


}