using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour {
    StateMachine stateMachine;
    Block myBlock;
    Job myJob;
    List<string> trackedJobs;
    Path_AStar myPath;
    Map map;

    public bool Unemployed
    {
        get { return (myJob == null); }
    }

    public Block Block {
        get { return myBlock; }
    }

    public Job Job {
        get { return myJob; }
    }

    private void Awake()
    {
        if (stateMachine == null) {
            stateMachine = gameObject.AddComponent<StateMachine>();
        }
        stateMachine.ChangeState<CSIdle>();
        trackedJobs = new List<string>();
        trackedJobs.Add(JobMetrics.MINING_JOB_TAG);
    }

    public void SetupCharacter(Block block, Map map)
    {
        SetBlock(block);
        this.map = map;
    }

    public void MoveToNewSpace(SquareDirection direction)
    {
        if (myBlock.GetNeighbor(direction) != null && myBlock.GetNeighbor(direction).IsWalkable)
            SetBlock(myBlock.GetNeighbor(direction));
    }

    public void MoveToNewSpace(Block block)
    {
        if (block != null)
            SetBlock(block);
    }

    private void SetBlock (Block block)
    {
        myBlock = block;

        transform.localPosition = myBlock.Point.ToCellPosition();
    }

    public void TrackJob(string job)
    {
        if(trackedJobs.Contains(job)==false)
            trackedJobs.Add(job);
    }

    public void StopTrackingJob(string job)
    {
        if(trackedJobs.Contains(job)==true)
            trackedJobs.Remove(job);
    }

    public int GetTrackedJobsListSize()
    {
        return trackedJobs.Count;
    }

    public string GetJobFromTrackedListByIndex(int index)
    {
        if (index < trackedJobs.Count)
            return trackedJobs[index];
        else
        {
            Debug.LogError("Requested Index out of range");
            return null;
        }
    }

    public void SetJob(Job j)
    {
        if (j == null)
            return;
        // Set my job
        myJob = j;
        // What if this job requires something?
        // Mining a block might require a mining pick (TOOL)
        // Building a door might require something to build the door out of (MATERIAL)
        // Might need multiple things (Carving a wood figurine might require a knife (TOOL) and a piece of wood (MATERIAL)
        // TODO: CHECK REQUIREMENTS STEP - If requirements aren't met, then we need to instead try to path
        // to where we can go to meet those requirements (ie, if we need a knife, we need to go find a knife)

        // Set my path (if we meet requirements, to the job location, if we don't, to required materials and tools)
        myPath = new Path_AStar(map, myBlock, myJob.GetTargetBlock());

        // Set my state (instead of move to job, maybe this should be 'move to location' state?)
        stateMachine.ChangeState<CSMoveToJob>();
    }

    public void ArrivedAtJob()
    {
        stateMachine.ChangeState<CSWorkJob>();
    }

    public Block GetNextPathBlock()
    {
        if (myPath.Length() > 0)
        {
            return myPath.GetNextBlock();
        }
        else
        {
            return null;
        }
    }

    public void GoIdle()
    {
        myJob = null;
        stateMachine.ChangeState<CSIdle>();
    }
}
