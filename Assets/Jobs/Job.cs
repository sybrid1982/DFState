using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Job  {
    protected string _jobTypeName;

    protected Block targetBlock;

    public string JobType
    {
        get { return _jobTypeName; }
    }

    public virtual void DoJob()
    {
        Debug.Log("Working job " + _jobTypeName);
    }

    public virtual Block GetTargetBlock()
    {
        return targetBlock;
    }
}
