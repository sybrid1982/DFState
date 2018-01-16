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
        Block nextMoveBlock = character.GetNextPathBlock();
        if (nextMoveBlock != null)
        {
            character.MoveToNewSpace(nextMoveBlock);
        } else
        {
            // If, after trying to get a map, there is still no map, then this job cannot
            // be reached and we should become idle again
            ReturnJob();
            character.GoIdle();
        }


        // If we're in a space that is a neighbor to the target space, then we should
        // bail into some sort of 'do job' state
        if (CheckIfAdjacentToJob())
        {
            character.ArrivedAtJob();
        }
    }

    private void ReturnJob()
    {
        // For now, destroy the job
        character.SetJob(null);
        // in the future, we will want to send it back to the joblist, perhaps with a notificiation
        // like "return_job" with the job being returned as the info
    }

    private bool CheckIfAdjacentToJob()
    {
        if(character.Block.IsNeighbor(character.Job.GetTargetBlock()))
            return true;

        return false;
    }
}
