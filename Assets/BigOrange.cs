﻿using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Tilemaps;
using System.Collections;

public class BigOrange : MonoBehaviour
{
    public Animator m_animator;
    public Text hpText;

    public string previousstate;

    public Slider slider;

    public FinalBossActivation activator;

    int rng;
    int rng2;
    int loopcount = 0;

    public float MaxHP = 100000;
    public float HP = 100000;

    private float[] speeds;

    private float revertcounter = 0;

    float HPpercentage;

    public Tilemap grid;
    public Tile changetile;
    public Tile defaulttile;

    private bool BlocksSpawnedlast = false;

    public Transform spawn1;
    public Transform spawn2;
    public Transform spawn3;
    public Transform spawn4;

    public Transform[] switches;

    public GameObject BossActivator;
    public GameObject Enemy1;
    public GameObject Enemy2;
    public GameObject Enemy3;
    public GameObject Enemy4;
    public GameObject Enemy5;
    public GameObject Enemy6;

    private IEnumerator RevertRoutine;

    int stompsinrow = 0;
    public int stompspeedindex = 0;

    bool justlooped = false;
    bool lastactionwasstomp = false;
    bool newRevertRoutine = false;


    // Start is called before the first frame update
    void Start()
    {
        m_animator = gameObject.GetComponent<Animator>();
        rng2 = 0;
        IdleLoops();

        HPpercentage = Mathf.Round(HP / MaxHP * 100) / 100;
        activator = BossActivator.GetComponent<FinalBossActivation>();

        speeds = new float[]
        {
            2.5f,
            1.25f,
            0.625f
        };
    }

    private void Update()
    {
        slider.maxValue = MaxHP;
        slider.minValue = 0;
        slider.value = HP;

        hpText.text = HP.ToString() + " / " + MaxHP.ToString();
        {

            if (HP <= 0)
            {
                m_animator.Play("Death");
                hpText.text = "0" + " / " + MaxHP.ToString();
            }
        }
    }

    void rngGenerate()
    {
        rng = Random.Range(1, 100);
        rng2 = Random.Range(1, 100);

        AnimationPlay();
    }

    void AnimationPlay()
    {
        if (rng <= 20)
        {
            m_animator.SetBool("Jump", true);
        }
        if (rng > 20 && rng <= 50)
        {
            m_animator.SetBool("LeftSlam", true);
        }
        if (rng > 50 && rng <= 80)
        {
            m_animator.SetBool("RightSlam", true);
        }
        if (rng > 80 && rng <= 100)
        {
            m_animator.SetBool("Stomp", true);
        }
    }

    void forceidle()
    {
        foreach (AnimatorControllerParameter parameter in m_animator.parameters)
        {
            if (parameter.type == AnimatorControllerParameterType.Bool)
            {
                m_animator.SetBool(parameter.name, false);
            }
        }
    }

    void IdleLoops()
    {
        justlooped = false;

        foreach (AnimatorControllerParameter parameter in m_animator.parameters)
        {
            if (parameter.type == AnimatorControllerParameterType.Bool)
            {
                m_animator.SetBool(parameter.name, false);
            }
        }


        HPpercentage = Mathf.Round(HP / MaxHP * 100) / 100;

        m_animator.SetBool("Idle", true);
        m_animator.SetFloat("Speed", 1 + (3 - (3 * HPpercentage)));

        if (loopcount == 0 && justlooped == false)
        {
            if (rng2 >= 50)
            {
                rngGenerate();
                loopcount = 0;
                m_animator.SetBool("Idle", false);
            }
            loopcount += 1;
            justlooped = true;
        }
        if (loopcount == 1 && justlooped == false)
        {
            if (rng2 >= 75)
            {
                rngGenerate();
                loopcount = 0;
                m_animator.SetBool("Idle", false);
            }
            loopcount += 1;
            justlooped = true;
        }

        if (loopcount == 2 && justlooped == false)
        {
            rngGenerate();
            loopcount = 0;
            m_animator.SetBool("Idle", false);
            justlooped = true;
        }
    }

    void SpawnEnemy1()
    {
        GameObject newenemy = Instantiate(Enemy1) as GameObject;
        EnemyMovement movement = newenemy.GetComponent<EnemyMovement>();
        newenemy.transform.position = spawn1.transform.position;
        movement.multiplier = 3;

        GameObject newenemy2 = Instantiate(Enemy1) as GameObject;
        EnemyMovement movement2 = newenemy2.GetComponent<EnemyMovement>();
        newenemy2.transform.position = spawn1.transform.position + new Vector3(-0.72f, 0, 0);
        movement2.multiplier = 3;

        GameObject newenemy3 = Instantiate(Enemy1) as GameObject;
        EnemyMovement movement3 = newenemy3.GetComponent<EnemyMovement>();
        newenemy3.transform.position = spawn1.transform.position + new Vector3(0.72f, 0, 0);
        movement3.multiplier = 3;

        GameObject newenemy4 = Instantiate(Enemy5) as GameObject;
        EnemyMovementTwo movement4 = newenemy4.GetComponent<EnemyMovementTwo>();
        newenemy4.transform.position = spawn3.transform.position;
        movement4.multiplier = 3;

        GameObject newenemy5 = Instantiate(Enemy5) as GameObject;
        EnemyMovementTwo movement5 = newenemy5.GetComponent<EnemyMovementTwo>();
        newenemy5.transform.position = spawn3.transform.position + new Vector3(0, -0.72f, 0);
        movement5.multiplier = 3;

        GameObject newenemy6 = Instantiate(Enemy5) as GameObject;
        EnemyMovementTwo movement6 = newenemy6.GetComponent<EnemyMovementTwo>();
        newenemy6.transform.position = spawn3.transform.position + new Vector3(0, 0.72f, 0);
        movement6.multiplier = 3;
    }

    void SpawnEnemy2()
    {
        GameObject newenemy = Instantiate(Enemy2) as GameObject;
        EnemyMovement movement = newenemy.GetComponent<EnemyMovement>();
        newenemy.transform.position = spawn2.transform.position;
        movement.multiplier = 3;

        GameObject newenemy2 = Instantiate(Enemy2) as GameObject;
        EnemyMovement movement2 = newenemy2.GetComponent<EnemyMovement>();
        newenemy2.transform.position = spawn2.transform.position + new Vector3(-0.72f, 0, 0);
        movement2.multiplier = 3;

        GameObject newenemy3 = Instantiate(Enemy2) as GameObject;
        EnemyMovement movement3 = newenemy3.GetComponent<EnemyMovement>();
        newenemy3.transform.position = spawn2.transform.position + new Vector3(0.72f, 0, 0);
        movement3.multiplier = 3;

        GameObject newenemy4 = Instantiate(Enemy6) as GameObject;
        EnemyMovementTwo movement4 = newenemy4.GetComponent<EnemyMovementTwo>();
        newenemy4.transform.position = spawn4.transform.position;
        movement4.multiplier = 3;

        GameObject newenemy5 = Instantiate(Enemy6) as GameObject;
        EnemyMovementTwo movement5 = newenemy5.GetComponent<EnemyMovementTwo>();
        newenemy5.transform.position = spawn4.transform.position + new Vector3(0, 0.72f, 0);
        movement5.multiplier = 3;

        GameObject newenemy6 = Instantiate(Enemy6) as GameObject;
        EnemyMovementTwo movement6 = newenemy6.GetComponent<EnemyMovementTwo>();
        newenemy6.transform.position = spawn4.transform.position + new Vector3(0, -0.72f, 0);
        movement6.multiplier = 3;
    }

    void SpawnEnemy3()
    {
        GameObject newenemy = Instantiate(Enemy3, spawn1.transform.position, Quaternion.identity);
        GameObject newenemy2 = Instantiate(Enemy4, spawn2.transform.position, Quaternion.identity);
    }

    void SpawnBlocks()
    {
        for (int i = 0; i < switches.Length; i++)
        {
            SetAdjacentTiles(switches[i], changetile);
        }

        if (RevertRoutine != null)
        {
            StopCoroutine(RevertRoutine);
        }
        RevertRoutine = RevertTiles(switches);
        StartCoroutine(RevertRoutine);
    }

    public void SetAdjacentTiles(Transform current, Tile targetTile)
    {
        float sideSize = 0.72f;
        if(current != null)
        {
            Vector3 currentCellPos = current.transform.position;
            Vector3Int[] adjacentCells = new Vector3Int[]
            {
                grid.WorldToCell(currentCellPos + Vector3.left * sideSize),
                grid.WorldToCell(currentCellPos + Vector3.up * sideSize),
                grid.WorldToCell(currentCellPos + Vector3.right * sideSize),
                grid.WorldToCell(currentCellPos + Vector3.down * sideSize),
            };
            foreach (Vector3Int adjacentCell in adjacentCells)
            {
                grid.SetTile(adjacentCell, targetTile);
            }
        }
    }

    public IEnumerator RevertTiles(Transform current){

        yield return new WaitForSeconds(speeds[stompspeedindex]);

        SetAdjacentTiles(current, null);

    }

    public IEnumerator RevertTiles(Transform[] current)
    {

        yield return new WaitForSeconds(speeds[stompspeedindex]);

        SetAdjacentTiles(current[0], null);
        SetAdjacentTiles(current[1], null);
        SetAdjacentTiles(current[2], null);
        SetAdjacentTiles(current[3], null);

    }

    public void BattleHasEnded()
    {
        activator.BattleEnd();
    }

    public void DestroySelf()
    {
        Destroy(gameObject);
    }
}