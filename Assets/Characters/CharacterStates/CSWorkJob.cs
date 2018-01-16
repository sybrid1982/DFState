using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CSWorkJob : CSState {
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

        // Returns true if job should continue
        if (character.Job.DoJob()==false)
        {
            character.GoIdle();
        }
    }

    private bool CheckIfAdjacentToJob()
    {
        if (character.Block.IsNeighbor(character.Job.GetTargetBlock()))
            return true;

        return false;
    }
}
