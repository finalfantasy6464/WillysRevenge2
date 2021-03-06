using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Events;

public class EnemyMovementTwo : MonoBehaviour, IPausable
{

    private float movespeed = 0.0f;
    private float defaultmovestep;
    private float movestep = 0.02f;
    private float moveinterval = 0.1f;
    public float movemagnitude = 1f;

    private float corruptionmod = 1.25f;
    public float multiplier = 1.0f;

    public Animator m_Animator;

    ParticleSystem particles;
    ParticleSystem.EmissionModule emissionModule;

    bool finished = false;



    private Vector2 enemydir = Vector2.zero;

    public bool StartsFalling;
    public bool lifespan = false;
    private bool justhit = false;
    private bool levelstart = false;

    public int direction = 1;

    private int hitcount;


    private float age = 0;
    public float death;

    [HideInInspector] public UnityEvent onWallHit;

    public bool isPaused { get; set; }

    void Awake()
    {
        onWallHit = new UnityEvent();
    }

    void Start()
    {
        particles = GetComponent<ParticleSystem>();
        emissionModule = particles.emission;
        defaultmovestep = movestep;
        StartCoroutine(LevelStarting());

        if (StartsFalling)
        {
            m_Animator.SetBool("isFalling", true);
        }
    }

    IEnumerator LevelStarting()
    {
        if (levelstart == false)
        {
            yield return new WaitForSeconds(0.2f);
            levelstart = true;
        }
    }

    void OnCollisionEnter2D(Collision2D coll)
    {

        while (direction == 1)
        {
            if (justhit == true)
            {
                break;
            }
            else
            {
                if (coll.gameObject.tag == "Enemy2")
                {
                    hitcount += 1;
                    direction = 2;
                    justhit = true;
                    m_Animator.SetBool("isFalling", false);
                }

                if (coll.gameObject.tag != "Teleport2" && coll.gameObject.tag != "Enemy2")
                {

                    direction = 2;
                    justhit = true;
                    onWallHit.Invoke();
                    m_Animator.SetBool("isFalling", false);
                }
                if (coll.gameObject.tag == "Teleport2")
                {

                    justhit = true;
                }
                if (coll.gameObject.tag == "Void")
                {
                    finished = true;
                    movestep = 0;
                    movespeed = 0;
                    m_Animator.Play("Enemy2Suction");
                }

                if(coll.gameObject.tag == "Tail")
                {
                    coll.gameObject.SetActive(false);
                }
            }
        }

        while (direction == 2)
        {
            if (justhit == true)
            {
                break;
            }
            else
            {

                if (coll.gameObject.tag == "Enemy2")
                {
                    hitcount += 1;
                    direction = 1;
                    justhit = true;
                    m_Animator.SetBool("isFalling", true);
                }

                if (coll.gameObject.tag != "Teleport2" && coll.gameObject.tag != "Enemy2")
                {

                    direction = 1;
                    justhit = true;
                    onWallHit.Invoke();
                    m_Animator.SetBool("isFalling", true);
                }

                if (coll.gameObject.tag == "Teleport2")
                {

                    justhit = true;
                }
                if (coll.gameObject.tag == "Void")
                {
                    finished = true;
                    movespeed = 0;
                    movestep = 0;
                    m_Animator.Play("Enemy2Suction");
                }
            }
        }
    }

    void OnTriggerEnter2D(Collider2D coll)
    {
        var hit = coll.gameObject;

        if (hit.CompareTag("Corruption"))
        {
            movestep = movestep * corruptionmod;
            emissionModule.rateOverTime = 10f;
        }
    }

    void OnTriggerExit2D(Collider2D coll)
    {
        var hit = coll.gameObject;
        if (hit.CompareTag("Corruption"))
        {
            movestep = defaultmovestep;
            emissionModule.rateOverTime = 0f;
        }
    }

    void Update()
    {
        if (!isPaused)
        {
            UnPausedUpdate();
        }
    }

    void LateUpdate()
    {
        justhit = false;
    }

    void Move()
    {

        Vector3 v = transform.position;

        transform.Translate(enemydir * movespeed);
    }

    void DestroySelf()
    {
        Destroy(this.gameObject);
    }


    public void OnPause()
    {}

    public void OnUnpause()
    {}

    public void OnDestroy()
    {
        PauseControl.TryRemovePausable(gameObject);
    }

    public void UnPausedUpdate()
    {
        if (lifespan == true)
        {
            age += Time.smoothDeltaTime;
            if (age >= death)
            {
                Destroy(gameObject);
            }
        }

        if (levelstart == true)
        {
            this.movespeed += movestep * multiplier;
        }

        if (this.movespeed >= moveinterval)
        {
            Move();
            movespeed = movespeed - moveinterval;
        }

        if (hitcount >= 10)
        {
            Destroy(gameObject);
        }

        if (!finished)
        {
            switch (direction)
            {

                case 2:
                    enemydir = Vector2.up * movemagnitude;
                    break;

                case 1:

                    enemydir = Vector2.down * movemagnitude;
                    break;
            }
        }
    }

    
}