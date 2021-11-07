using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ExitCollision : MonoBehaviour
{

    public int currentlevelID;

    Scene m_Scene;
    public string sceneName;
    public int nocomplete;
    public bool finalexit = false;

    public AudioClip clearsound;

    PlayerController2021remake playercontroller;
    GameObject player;

    LevelTimer leveltimer;
    GameObject timetracker;

    GameObject SoundManager;
    MusicManagement music;

    private string musicVolume;

    private Collider2D playercoll;

    public Canvas endlevelcanvas;
    public CanvasGroup levelcanvasgroup;

    void Start()
    {
        if(Time.timeScale == 0)
        {
            Time.timeScale = 1;
        }

        SoundManager = GameObject.Find("SoundManager");

        music = SoundManager.GetComponent<MusicManagement>();
        music.onLevelStart.Invoke();

        musicVolume = MusicManagement.MUSIC_VOLUME;

        player = GameObject.FindGameObjectWithTag("Player");
        timetracker = GameObject.Find("Timer");
        playercontroller = player.GetComponent<PlayerController2021remake>();
        playercoll = player.GetComponent<Collider2D>();
        leveltimer = timetracker.GetComponent<LevelTimer>();

        m_Scene = SceneManager.GetActiveScene();
        sceneName = m_Scene.name;
        currentlevelID = int.Parse(sceneName.Substring(5));

        GameControl.control.savedPinID = currentlevelID;
        GameControl.control.lastSceneWasLevel = true;
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
                GameSoundManagement.instance.PlayOneShot(clearsound);
                StartCoroutine(FadeAudioGroup.StartFade(music.audioMixer, musicVolume, 0.5f, 0f));
                GameControl.control.faded = true;
                playercontroller.canmove = false;
                playercontroller.dirlock = true;
            }

            GameControl.CompletedLevelCheck(currentlevelID, playercontroller.gotgold, leveltimer.expired);
            GameControl.control.CompletionPercentageCheck();
            GameControl.control.AutoSave();
    }

    private void ShowLevelCompleteWindow()
    {
        levelcanvasgroup.gameObject.SetActive(true);
        levelcanvasgroup.alpha = 1;
        levelcanvasgroup.interactable = true;
    }
}