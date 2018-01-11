using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JobList  {
    Job _jobType;
    Queue<Job> availableJobs;
    Dictionary<Job, Character> assignedJobs;

    public JobList (Job jobType)
    {
        _jobType = jobType;
    }

    public Job GetNextJobInQueue(Character worker)
    {
        if (availableJobs.Count == 0)
            return null;
        else
        {
            Job job = availableJobs.Dequeue();
            if (assignedJobs == null)
                assignedJobs = new Dictionary<Job, Character>();

            assignedJobs.Add(job, worker);
            return job;
        }
    }

    public bool AddJobToQueue(Job job)
    {
        if (job.JobType != _jobType.JobType)
        {
            return false;
        }
        else
        {
            availableJobs.Enqueue(job);
            return true;
        }
    }
}
