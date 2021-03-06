using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SwitchCollide : MonoBehaviour
{

	public Sprite defaultsprite;
	public Sprite changesprite;
    public bool activated = false;
    public bool usesToggle = true;

    public AudioClip switchhit;

    public BoolEvent onSwitchToggle = new BoolEvent();

    void OnTriggerEnter2D(Collider2D coll) {
		if (coll.gameObject.tag == "Player") {
			this.GetComponent<SpriteRenderer>().sprite = changesprite;
            toggle();
		}
	}

    public void toggle()
    {
        if (!usesToggle && activated == false)
        {
            activated = true;
            onSwitchToggle.Invoke(activated);
            GameSoundManagement.instance.PlaySingle(switchhit);
        }
        else if (usesToggle)
        {
            activated = !activated;
            onSwitchToggle.Invoke(activated);
            GameSoundManagement.instance.PlaySingle(switchhit);
        }

    }
}

public class BoolEvent : UnityEvent<bool>
{}