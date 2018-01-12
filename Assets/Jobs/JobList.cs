using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* Thoughts: Right now this is set up where you'd have a job list for every
 * job type.  What if we instead made the job list a class where there would
 * only be one, but then the joblist class makes individual lists for each
 * job type?  Seems like that should be a more sensible solution. */
public class JobList : MonoBehaviour  {
    Dictionary<Job, Character> assignedJobs;

    Dictionary<string, Queue<Job>> jobNameToJobQueueMap;

    private void Awake()
    {
        NotificationExtensions.AddObserver(this, CreateMiningJob, JobMetrics.MINING_JOB_POST_NOTICE);
        jobNameToJobQueueMap = new Dictionary<string, Queue<Job>>();
    }

    public Job GetNextJobInQueue(string jobType, Character worker)
    {
        if (jobNameToJobQueueMap.ContainsKey(jobType))
        {
            Queue<Job> availableJobs = jobNameToJobQueueMap[jobType];

            if (availableJobs == null || availableJobs.Count == 0)
                return null;
            else
            {
                Job job = (Job)availableJobs.Dequeue();
                if (assignedJobs == null)
                    assignedJobs = new Dictionary<Job, Character>();

                assignedJobs.Add(job, worker);
                return job;
            }
        } else
        {
            return null;
        }
    }

    public void AddJobToQueue(Job job)
    {
        Queue<Job> jobQueue;
        
        if (jobNameToJobQueueMap.ContainsKey(job.JobType) == false)
            jobQueue = new Queue<Job>();
        else
            jobQueue = jobNameToJobQueueMap[job.JobType];

        
        if(CheckQueueForIdenticalJob(jobQueue, job))
        {
            return;
        }
        
        jobQueue.Enqueue(job);
        Debug.Log("Created job added to jobQueue " + job.JobType + " at index " + jobQueue.Count);
    }

    void CreateMiningJob(object sender, object target)
    {
        MiningJob mj = new MiningJob((Block)target);
        AddJobToQueue(mj);
    }

    bool CheckQueueForIdenticalJob(Queue<Job> queue, Job job)
    {
        foreach(Job j in queue)
        {
            if(j.GetTargetBlock() == job.GetTargetBlock())
            {
                return true;
            }
        }
        return false;
    }
}
