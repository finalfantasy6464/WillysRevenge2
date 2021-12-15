﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorSwitcher : MonoBehaviour
{

	public int type = 1;

	GameObject[] Colourdoor;

	private ColouredDoor[] door;

    private bool justchanged;

    public Sprite initialsprite;
    public Sprite[] colours;

    public SpriteRenderer m_spriterenderer;

    public AudioClip switchhit;

    // Start is called before the first frame update
    void Start()
    {
		Colourdoor = GameObject.FindGameObjectsWithTag ("Colour");
		door = new ColouredDoor[Colourdoor.Length];
        m_spriterenderer.sprite = initialsprite;
        justchanged = false;
}

	void OnTriggerEnter2D(Collider2D coll)
    {
		var hit = coll.gameObject;

		if (hit.tag == "Player" & type == 1) {

            ChangeSprite();
            GameSoundManagement.instance.PlaySingle(switchhit);

            for (int i = 0; i < Colourdoor.Length; i++) {
				door [i] = Colourdoor [i].GetComponent<ColouredDoor> ();
				door [i].Red = !door [i].Red;
			}
		}

		if (hit.tag == "Player" & type == 2) {

           ChangeSprite();
           GameSoundManagement.instance.PlaySingle(switchhit);

            for (int i = 0; i < Colourdoor.Length; i++) {
				door [i] = Colourdoor [i].GetComponent<ColouredDoor> ();
				door [i].Green = !door [i].Green;
			}
        }
    }

    void OnTriggerExit2D(Collider2D coll)
    {
        if(coll.gameObject.tag == "Player")
        {
            justchanged = false ;
        }
    }

    void ChangeSprite()
    {
        if (m_spriterenderer.sprite == colours[0] && justchanged == false)
        {
            m_spriterenderer.sprite = colours[1];
            justchanged = true;
        }

        if (m_spriterenderer.sprite == colours[1] && justchanged == false)
        {
            m_spriterenderer.sprite = colours[0];
            justchanged = true;
        }

        if (m_spriterenderer.sprite == colours[2] && justchanged == false)
        {
            m_spriterenderer.sprite = colours[3];
            justchanged = true;
        }

        if (m_spriterenderer.sprite == colours[3] && justchanged == false)
        {
            m_spriterenderer.sprite = colours[2];
            justchanged = true;
        }
    }
}
