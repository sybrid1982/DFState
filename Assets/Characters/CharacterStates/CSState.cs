using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CSState : State {
    protected const string UpdateNotice = "TICK_POSTED";
    protected virtual void OnUpdateTick(object sender, object info)
    {

    }
}