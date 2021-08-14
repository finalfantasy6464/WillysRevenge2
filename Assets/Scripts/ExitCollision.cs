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

    PlayerController playercontroller;
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
        playercontroller = player.GetComponent<PlayerController>();
        playercoll = player.GetComponent<Collider2D>();
        leveltimer = timetracker.GetComponent<LevelTimer>();

        m_Scene = SceneManager.GetActiveScene();
        sceneName = m_Scene.name;
        currentlevelID = int.Parse(sceneName.Substring(5));

    }

    void OnCollisionEnter2D(Collision2D col)
    {
            var hit = col.gameObject;

            if (hit.tag == "Player")
            {

                if (finalexit == true)
                {
                    SceneManager.LoadScene("Credits");
                    GameControl.control.bosscheckpoint = false;

                    if (GameControl.control.completedlevels[currentlevelID] == false)
                    {
                        GameControl.control.completedlevels[currentlevelID] = true;
                        GameControl.control.complete += 1;

                    }
                    if (GameControl.control.goldenpellets[currentlevelID] == false && playercontroller.gotgold == true)
                    {
                        GameControl.control.goldenpellets[currentlevelID] = true;
                        GameControl.control.golden += 1;
                    }

                    if (GameControl.control.timerchallenge[currentlevelID] == false && leveltimer.expired == false)
                    {
                        GameControl.control.timerchallenge[currentlevelID] = true;
                        GameControl.control.timer += 1;
                    }

                    GameControl.control.AutoSave();
                }
                else
                {
                    levelcanvasgroup.gameObject.SetActive(true);
                    levelcanvasgroup.alpha = 1;
                    levelcanvasgroup.interactable = true;

                    GameSoundManagement.instance.PlayOneShot(clearsound);

                StartCoroutine(FadeAudioGroup.StartFade(music.audioMixer, musicVolume, 0.5f, 0f));
                GameControl.control.faded = true;

                    playercontroller.presentdir = 5;
                    playercontroller.dirlock = true;

                    GameControl.control.bosscheckpoint = false;

                    if (GameControl.control.completedlevels[currentlevelID] == false)
                    {
                        GameControl.control.completedlevels[currentlevelID] = true;
                        GameControl.control.complete += 1;

                    }
                    if (GameControl.control.goldenpellets[currentlevelID] == false && playercontroller.gotgold == true)
                    {
                        GameControl.control.goldenpellets[currentlevelID] = true;
                        GameControl.control.golden += 1;
                    }

                    if (GameControl.control.timerchallenge[currentlevelID] == false && leveltimer.expired == false)
                    {
                        GameControl.control.timerchallenge[currentlevelID] = true;
                        GameControl.control.timer += 1;
                    }

                    GameControl.control.AutoSave();
                }
            }
        }
    }