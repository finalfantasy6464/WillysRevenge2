using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShiftScriptTwo : MonoBehaviour
{
	private PlayerController2021remake playercontroller;
    public float pushstrengthHoriz = 0.1f;
    public float pushstrengthVert = 0.1f;
	public Vector3 pushVector = Vector3.zero;
	public float oppositeMagnitude = 0.5f;

	void OnTriggerStay2D(Collider2D sand)
	{
		if(sand.gameObject.TryGetComponent(out PlayerController2021remake player))
        {
			playercontroller = player;
			SetShiftVector(pushstrengthHoriz, pushstrengthVert);
		}
	}

	void OnTriggerExit2D(Collider2D sand)
	{
        if (sand.gameObject.TryGetComponent(out PlayerController2021remake player) && player == playercontroller)
		{
			SetShiftVector(0f, 0f);
        }
	}

	void SetShiftVector(float x, float y)
    {
		pushVector = new Vector3(x, y, 0);

		if(Vector3.Dot(pushVector, playercontroller.direction) <= 0)
        {
			pushVector = new Vector3(pushVector.x * oppositeMagnitude, pushVector.y * oppositeMagnitude, 0);
        }
		playercontroller.shiftVector = pushVector;
    }
}