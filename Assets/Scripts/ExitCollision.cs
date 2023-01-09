using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

public class ExitCollision : MonoBehaviour
{
    public int currentlevelID;

    Scene m_Scene;
    public string sceneName;
    public int nocomplete;
    public bool finalexit = false;
    public GameObject exitLight;

    public AudioClip clearsound;

    PlayerController2021remake playercontroller;
    GameObject player;

    LevelTimer leveltimer;

    TimeSpan parTimeSecs;

    GameObject timetracker;

    GameObject SoundManager;
    MusicManagement music;

    private string musicVolume;

    private Collider2D playercoll;

    private LevelTimer timer;

    public LevelList levelList;
    public LevelCanvas InLevelCanvas;
    public Canvas endlevelcanvas;
    public CanvasGroup levelcanvasgroup;

    void Start()
    {
        if(Time.timeScale == 0)
        {
            Time.timeScale = 1;
        }

        if(InLevelCanvas == null)
        InLevelCanvas = GameObject.FindObjectOfType<LevelCanvas>();

        SoundManager = GameObject.Find("SoundManager");

        music = SoundManager.GetComponent<MusicManagement>();
        music.onLevelStart.Invoke();

        musicVolume = MusicManagement.MUSIC_VOLUME;
        timer = FindObjectOfType<LevelTimer>();

        player = GameObject.FindGameObjectWithTag("Player");
        timetracker = GameObject.Find("Timer");
        playercontroller = player.GetComponent<PlayerController2021remake>();
        playercoll = player.GetComponent<Collider2D>();
        if(timetracker != null)
        leveltimer = timetracker.GetComponent<LevelTimer>();

        m_Scene = SceneManager.GetActiveScene();
        sceneName = m_Scene.name;
        currentlevelID = int.Parse(sceneName.Substring(5));


        SetLevelInfo();
        GameControl.control.savedPinID = currentlevelID;
        GameControl.control.lastSceneWasLevel = true;
    }

    public void SetLevelInfo()
    {
        InLevelCanvas.levelNameText.SetText(levelList.levelNames[currentlevelID - 1]);
        parTimeSecs = TimeSpan.FromSeconds(levelList.parTimes[currentlevelID - 1]);

        if (parTimeSecs.TotalSeconds >= 60)
            InLevelCanvas.parTimeText.SetText("Time To Beat: " + parTimeSecs.Minutes.ToString() + " minute, " + parTimeSecs.Seconds.ToString() + " seconds");
        else
            InLevelCanvas.parTimeText.SetText("Time To Beat: " + parTimeSecs.Seconds.ToString() + " seconds");
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag != "Player")
            return;

        if (finalexit)
            SceneManager.LoadScene("Credits");
        else
        {
            ShowLevelCompleteWindow();
            timer.isCounting = false;
            GameSoundManagement.instance.PlayOneShot(clearsound);
            StartCoroutine(FadeAudioGroup.StartFade(music.audioMixer, musicVolume, 0.5f, 0f));
            GameControl.control.faded = true;
            playercontroller.ResetMovement();
            playercontroller.canmove = false;
            playercontroller.dirlock = true;
        }

        GameControl.CompletedLevelCheck(currentlevelID, playercontroller.gotgold, leveltimer.expired);
        GameControl.control.CompletionPercentageCheck();
        GameControl.control.AutoSave();
        if(!finalexit)
        endlevelcanvas.GetComponent<EndLevelCanvas>().UpdateEmblemStatus();
    }

    private void OnTriggerEnter2D(Collider2D coll)
    {
        var hit = coll.gameObject;

        if (hit.CompareTag("Tail") && playercontroller.canmove == false)
        {
            hit.GetComponent<Animator>().Play("SegmentExit");
        }
    }

    private void ShowLevelCompleteWindow()
    {
        levelcanvasgroup.alpha = 1;
        levelcanvasgroup.interactable = true;
        levelcanvasgroup.blocksRaycasts = true;
        endlevelcanvas.GetComponent<EndLevelCanvas>().FocusRetryButton();
        exitLight.SetActive(false);
    }
}