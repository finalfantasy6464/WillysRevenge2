using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Events;

public class PlayerController2021remake : MonoBehaviour
{
    public bool shieldactive = false;

    public bool dirlock = false;
    public bool gotgold = false;

    public List<follower> taillist = new List<follower>();
    public GameObject tailPrefab;
    public GameObject Shield;

    // movement
    public Vector2 dir = Vector2.zero;
    public int presentdir;
    public Vector2 pushback = Vector2.zero;


    [HideInInspector]
    public float pelletno = 0.0f;
    

    public Sprite Sprite1;
    public Sprite Sprite2;
    public Sprite Sprite3;
    public Sprite Sprite4;
    

    private int count;
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
    }

    void Start()
    {
        count = 0;
        dirlock = false;
        SetCountMax();
        SetCountText();
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