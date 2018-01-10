using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSpeedUI : MonoBehaviour {
    UpdateController uc;

    private void Start()
    {
        uc = FindObjectOfType<UpdateController>();
    } 
    public void ChangeGameSpeed(float newSpeed)
    {
        if ((int)newSpeed == 0)
            uc.ChangeState<PauseState>();
        else
        {
            uc.ChangeState<RunState>();
            uc.Speed = (UpdateController.GameSpeed)(int)newSpeed;
        }
    }
}
