using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitialStart : MonoBehaviour
{
    public void ActivateStart()
    {
        GameControl.control.InitialGameStarted = true;
        GameControl.control.AutoSave();
    }
}
