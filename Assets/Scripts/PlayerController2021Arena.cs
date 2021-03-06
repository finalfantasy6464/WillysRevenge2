using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Events;
using System;

public class PlayerController2021Arena : MonoBehaviour, IPausable
{
    //Otherobjects
    public bool dirlock = true;

    private float pelletgap;
    Rigidbody2D rb;

    public float Score;
    public float scoreCounter;
    public float scoreTick;

    public bool scoreLock;

    public ArenaController arena;
    public LevelTimerArena timer;

    [HideInInspector] public SpriteRenderer spriteRenderer;

    //Segments
    public List<GameObject> taillist = new List<GameObject>();
    private List<Vector3> PositionHistory = new List<Vector3>();
    public List<Vector3> slowdownPositionCache = new List<Vector3>();
    public int followDelay;
    public int speedIncrease;
    public bool debugShowFromStart;
    public Sprite[] SegmentSprites;
    public int spriteValue;
    public int offsetFromHead;
    public int LatestSegmentDelay;
    public int SegmentDelaySleep = 5;
    public int SegmentDelaySleepcache;
    public int SegmentDelaySleepCounter;
    public int SegmentHistorySkipCounter;
    public int SegmentHistorySkipTimer;
    public bool ate;
    public bool isSegmentJumping;
    public Vector3 LastLandPosition;

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
    private float dpadX;
    private float dpadY;

    public float pelletno = 0.0f;

    private int Scorecount;

    private int numberofpickups;

    public Text countText;
    public Text speedText;

    public Image GoldenPellet;

    [HideInInspector] public UnityEvent onEatPellet;
    [HideInInspector] public UnityEvent onGoldenPellet;
    [HideInInspector] public UnityEvent onSlidingDoorOpen;
    [HideInInspector] public UnityEvent onSlidingDoorClose;
    [HideInInspector] public UnityEvent onCollectShield;

    public bool isPaused { get ; set ; }

    void Awake()
    {
        onEatPellet = new UnityEvent();
        onGoldenPellet = new UnityEvent();
        onSlidingDoorOpen = new UnityEvent();
        onSlidingDoorClose = new UnityEvent();
        onCollectShield = new UnityEvent();

        for (int i = 0; i < 80; i++) 
        {
            slowdownPositionCache.Add(new Vector3(99,99,99));
        }

        SegmentDelaySleepcache = SegmentDelaySleep;
    }

    void Start()
    {
        Scorecount = 0;
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
        if(!isPaused)
            UnPausedUpdate();
    }

    void UpdateHistoryLists()
    {
        PositionHistory.Insert(0, transform.position);

        if(PositionHistory.Count > 1000)
        {
            PositionHistory.RemoveRange(1000, PositionHistory.Count - 1000);
        }
    }

    public void enableMovement()
    {
        canmove = true;
        dirlock = false;
    }

    void Move()
    {
        horizontal = Input.GetAxisRaw("Horizontal");
        vertical = Input.GetAxisRaw("Vertical");
        dpadX = Input.GetAxisRaw("DpadX");
        dpadY = Input.GetAxisRaw("DpadY");


        if(horizontal == -1 && direction.x == 1
                || horizontal == 1 && direction.x == -1
                || vertical == -1 && direction.y == 1
                || vertical == 1 && direction.y == -1)
        {
            horizontal = direction.x;
            vertical = direction.y;
        }

        if ((vertical != 0 || horizontal != 0 || dpadX != 0 || dpadY != 0) && dirlock == false)
        {
            if (horizontal > 0 || dpadX > 0)
            {
                direction.x = 1;
                direction.y = 0;
                rotationAngle = 0;
            }
            else if (horizontal < 0 || dpadX < 0)
            {
                direction.x = -1;
                direction.y = 0;
                rotationAngle = 180;
            }
            else if (vertical > 0 || dpadY > 0)
            {
                direction.y = 1;
                direction.x = 0;
                rotationAngle = 90;
            }
            else if (vertical < 0 || dpadY < 0)
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

    void UpdateSegmentSprite(Sprite segmentsprite)
    {
        SpriteRenderer currentrenderer;
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
    }
    void UpdateSegmentTint()
    {
        SpriteRenderer currentrenderer;
        
        for (int i = 0; i < taillist.Count; i++)
        {
            currentrenderer = taillist[i].GetComponent<SpriteRenderer>();
            currentrenderer.color = new Color(1f - (i * 0.007f), 1f - (i * 0.007f), 1f - (i * 0.007f), 1f);
           
            if (!taillist[i].activeInHierarchy)
                break;
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
        UpdateSegmentSprite(segmentsprite);
        UpdateSegmentTint();
    }

    public void SegmentSetter()
    {
        for(int i = 0; i < taillist.Count - 1; i++)
        {
            if(!taillist[i].activeInHierarchy && taillist[i + 1].activeInHierarchy)
            {
                taillist[i + 1].SetActive(false);
                pelletno--;
            }
        }

        if(pelletno < 0)
        {
            pelletno = 0;
        }
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

            if (i == taillist.Count - 1 && taillist[i].transform.position == LastLandPosition)
            {
                    isSegmentJumping = false;
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

    void UpdateSegmentOrder()
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
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Pickup"))
        {
            onEatPellet.Invoke();
            other.gameObject.SetActive(false);
            pelletgap = 0;
            pelletno++;
            Scorecount = Scorecount + 1;
            arena.SpawnAny();

            SetCountText();
            if (!debugShowFromStart)
            {
                ShowNextSegment(SegmentSprites[0], 1);
            }
        } else if(other.gameObject.name == "TimerClock")
        {
            other.gameObject.SetActive(false);
            timer.currentLevelTime += 15f;
            arena.SpawnAny();
        }
    }

    void SetCountText()
    {
        countText.text = "Score: " + Score.ToString();
    }

    void SetSpeedText()
    {
        if(speedText != null)
        {
            if (direction == Vector3.zero)
            {
                speedText.text = "Speed: " + 0 + " Mph";
            }
            else
            {
                speedText.text = "Speed: " + finalspeed.ToString() + " MPH";
            }
        }
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

    public void OnPause()
    {
        isPaused = true;
    }

    public void OnUnpause()
    {
        isPaused = false;
    }

    public void UnPausedUpdate()
    {
        Move();
        SetSpeedText();
        UpdateScore();
        UpdateSegmentsPosition();
        UpdateSegmentDelay();
        UpdateSegmentOrder();
        DebugOptionsCheck();
        UpdateHistoryLists();

        this.pelletgap += Time.deltaTime;

        
    }

    public void UpdateScore()
    {
        if (!scoreLock)
        {
            scoreCounter += Time.deltaTime;

            if (scoreCounter >= scoreTick)
            {
                Score += (1 * pelletno);
                SetCountText();
                scoreCounter = 0;
            }
        }
    }

    public void OnDestroy()
    {
        PauseControl.TryRemovePausable(gameObject);
    }
}
