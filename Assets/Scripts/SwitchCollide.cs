using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SwitchCollide : MonoBehaviour
{

	public Sprite defaultsprite;
	public Sprite changesprite;
    public bool activated = false;

    public AudioClip switchhit;

    public BoolEvent onSwitchToggle = new BoolEvent();

    void OnTriggerEnter2D(Collider2D coll) {
		if (coll.gameObject.tag == "Player") {
			this.GetComponent<SpriteRenderer>().sprite = changesprite;
            GameSoundManagement.instance.PlaySingle(switchhit);
            toggle();
		}
	}

    public void toggle()
    {
        activated = !activated;
        onSwitchToggle.Invoke(activated);
    }
}

public class BoolEvent : UnityEvent<bool>
{}