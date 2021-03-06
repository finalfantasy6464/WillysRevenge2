using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchCollide2 : MonoBehaviour
{

	private SwitchCollide switchcollide;
	GameObject Switch;
    Collider2D mycollider;

    public AudioClip switchhit;

    public Sprite activatedSprite;
    public Sprite deactivatedSprite;

	void Start(){
		Switch = GameObject.FindGameObjectWithTag ("Switch");
		switchcollide = Switch.GetComponent<SwitchCollide> ();
        switchcollide.onSwitchToggle.AddListener(onCheck);
        mycollider = GetComponent<Collider2D>();
        mycollider.enabled = false;
	}

    void onCheck(bool value)
    {
        if (value)
        {
            mycollider.enabled = true;
            GetComponent<SpriteRenderer>().sprite = deactivatedSprite;
        }
    }
	void OnTriggerEnter2D(Collider2D coll) {
		
		if (coll.gameObject.tag == "Player") {
			switchcollide.GetComponent<SpriteRenderer>().sprite = switchcollide.defaultsprite;
            switchcollide.onSwitchToggle.Invoke(false);
            switchcollide.activated = false;
            mycollider.enabled = false;
            GameSoundManagement.instance.PlaySingle(switchhit);
            GetComponent<SpriteRenderer>().sprite = activatedSprite;
		}
	}
		}