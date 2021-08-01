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

    PlayerController playercontroller;
    GameObject player;

    LevelTimer leveltimer;
    GameObject timetracker;

    private Collider2D playercoll;

    public Canvas endlevelcanvas;
    public CanvasGroup levelcanvasgroup;

    void Start()
    {
        if(Time.timeScale == 0)
        {
            Time.timeScale = 1;
        }
        GameObject.Find("SoundManager").GetComponent<MusicManagement>().onLevelStart.Invoke();
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


        if (GameControl.control.returntoselect == true)
        {
            GameControl.control.returntoselect = false;
            SceneManager.LoadScene("MainMenu");

        }
        else
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
}