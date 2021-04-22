﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RadialActivate : MonoBehaviour
{
    public RadialGauge radial;

    Collider2D coll;
    SpriteRenderer s_renderer;
    Sprite newsprite;
    Color newcolor;

    RadialProgress progress;

    public int boulderamount = 0;

    public bool justspawned = false;

    public GameObject boulder;

    public Transform spawn;

    private float collenable;

    private void Start()
    {
        radial.gameObject.SetActive(false);
        radial.enabled = false;
        radial.CurrentValue = 0;
        coll = GetComponent<Collider2D>();
        s_renderer = GetComponent<SpriteRenderer>();
        newcolor = s_renderer.color;
    }

    private void Update()
    {
        if (coll.enabled == false && coll != null)
        {
            collenable += Time.deltaTime;

            if (collenable >= 2.5f)
            {
                collenable = 0;
                coll.enabled = true;
                newcolor.a = 1;
                s_renderer.color = newcolor;
                justspawned = false;
            }
        }
    }
    // Start is called before the first frame update

    private void OnTriggerEnter2D(Collider2D collision)
    {
        var hit = collision.gameObject;

        if (hit.tag == "Player")
        {
            radial.gameObject.SetActive(true);
            radial.enabled = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        var hit = collision.gameObject;
        if (hit.tag == "Player")
        {
            radial.gameObject.SetActive(false);
            radial.enabled = false;
            radial.CurrentValue = 0;
        }
    }

    public void TriggerBoulder()
    {
        if(justspawned == false)
        {
            GameObject newboulder = Instantiate(boulder) as GameObject;
            boulder.transform.position = spawn.transform.position;
            boulderamount += 1;
            coll.enabled = false;
            newcolor.a = 0.25f;
            s_renderer.color = newcolor;
            justspawned = true;
        }
    }
}
