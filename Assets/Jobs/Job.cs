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

    public virtual bool DoJob()
    {
        Debug.Log("Working job " + _jobTypeName);
        return false;
    }

    public virtual Block GetTargetBlock()
    {
        return targetBlock;
    }
}
