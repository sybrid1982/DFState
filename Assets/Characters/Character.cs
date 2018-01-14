using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour {
    StateMachine stateMachine;
    Block myBlock;
    Job job;
    List<string> trackedJobs;

    public bool Unemployed
    {
        get { return (job == null); }
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

    public void SetupCharacter(Block block)
    {
        SetBlock(block);
    }

    public void MoveToNewSpace(SquareDirection direction)
    {
        Debug.Log("Asked to move " + direction.ToString());
        if (myBlock.GetNeighbor(direction) != null)
            SetBlock(myBlock.GetNeighbor(direction));
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
        job = j;
        Debug.Log("Got Job!");
        stateMachine.ChangeState<CSMoveToJob>();
    }
}
