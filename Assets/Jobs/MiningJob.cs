using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiningJob : Job {

    public MiningJob(Block block)
    {
        _jobTypeName = JobMetrics.MINING_JOB_STRING;
        targetBlock = block;
    }
}
