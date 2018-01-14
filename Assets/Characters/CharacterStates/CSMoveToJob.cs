using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CSMoveToJob : CSState {
    Character character;

    public override void Enter()
    {
        base.Enter();
        character = transform.root.GetComponent<Character>();
    }

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
        // If the character doesn't have a map to the target space, get one
        
        // If, after trying to get a map, there is still no map, then this job cannot
        // be reached and we should become idle again

        // If we're in a space that is a neighbor to the target space, then we should
        // bail into some sort of 'do job' state

        // Finally, if we get to here, then we should have a map and should be able to
        // move to the target space and should do so
    }

}
