using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CSIdle : CSState {
    Character character;
    JobList jobqueue;

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
        // Check for urgent needs
        // Then check for jobs this character should be doing
        // This should maybe be a list of strings that we check for?
        // Like, JobMetrics contains const strings, so we could have a list here for jobs to check
        // And then go through the queue
        // This job list should be on the character, not the state
        // Maybe be a priority queue so we can rank them in priority
        if(character.Unemployed)
            SearchForJob();

        // Then check for whether this character is in a social space
        // Then just move to a space in one of the four cardinal directions
        MoveToRandomSpace();
    }

    private void MoveToRandomSpace()
    {
        int random = Random.Range(0, 4);
        random *= 2;
        SquareDirection moveDirection = (SquareDirection)random;
        character.MoveToNewSpace(moveDirection);
    }

    private void SearchForJob()
    {
        if(character.GetTrackedJobsListSize() > 0)
        {
            for (int i = 0; i < character.GetTrackedJobsListSize(); i++)
            {
                string jobType = character.GetJobFromTrackedListByIndex(i);
                if (GetJob(jobType))
                    return;
            }
        }
    }

    private bool GetJob(string jobType)
    {
        // If we don't have the job queue, find it
        if (jobqueue == null)
            jobqueue = FindObjectOfType<JobList>();

        if (jobqueue == null)
        {
            Debug.LogError("No JobList found to GetJobs");
            return false;
        }
        // If we get a job, return true
        Job j = jobqueue.GetNextJobInQueue(jobType, character);
        if (j != null)
        {
            Debug.Log("Got a job!");
            character.SetJob(j);
            return true;
        }

        // Otherwise return false
        return false;
    }
}