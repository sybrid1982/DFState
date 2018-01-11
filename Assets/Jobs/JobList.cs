using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* Thoughts: Right now this is set up where you'd have a job list for every
 * job type.  What if we instead made the job list a class where there would
 * only be one, but then the joblist class makes individual lists for each
 * job type?  Seems like that should be a more sensible solution. */
public class JobList  {
    Dictionary<Job, Character> assignedJobs;

    Dictionary<string, Queue<Job>> jobNameToJobQueueMap;

    public Job GetNextJobInQueue(string jobType, Character worker)
    {
        Queue<Job> availableJobs = jobNameToJobQueueMap[jobType];

        if (availableJobs==null || availableJobs.Count == 0)
            return null;
        else
        {
            Job job = (Job)availableJobs.Dequeue();
            if (assignedJobs == null)
                assignedJobs = new Dictionary<Job, Character>();

            assignedJobs.Add(job, worker);
            return job;
        }
    }

    public void AddJobToQueue(Job job)
    {
        Queue<Job> jobQueue;
        if (jobNameToJobQueueMap.ContainsKey(job.JobType) == false)
            jobQueue = new Queue<Job>();
        else
            jobQueue = jobNameToJobQueueMap[job.JobType];

        jobQueue.Enqueue(job);
    }
}
