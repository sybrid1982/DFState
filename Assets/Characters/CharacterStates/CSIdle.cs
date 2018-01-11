using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CSIdle : CSState {
    protected override void AddListeners()
    {
        base.AddListeners();
        NotificationExtensions.AddObserver(this, OnUpdateTick, UpdateNotice);
    }

    protected override void RemoveListeners()
    {
        base.RemoveListeners();
        NotificationExtensions.RemoveObserver(this, OnUpdateTick, UpdateNotice);
    }

    protected override void OnUpdateTick(object sender, object info)
    {
        base.OnUpdateTick(sender, info);
        Debug.Log("Character recieved update");
    }
}