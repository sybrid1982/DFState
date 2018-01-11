using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour {
    StateMachine stateMachine;

    private void Awake()
    {
        stateMachine.ChangeState<CSIdle>();
    }
}
