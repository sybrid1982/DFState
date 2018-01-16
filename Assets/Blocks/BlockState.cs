using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BlockState {
    protected int _moveCost;
    protected int _health;
    protected BlockContents _contents;
    protected bool hasFloor = true;
    public int MoveCost {
        get { return _moveCost; }
    }
    public BlockContents Contents
    {
        get { return _contents; }
    }
}
