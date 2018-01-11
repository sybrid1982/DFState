using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiningJob : Job {

    public MiningJob(Block block)
    {
        _jobTypeName = "MINING";
        targetBlock = block;
    }
}
