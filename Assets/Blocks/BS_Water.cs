using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BS_Water : BlockState {
    public BS_Water()
    {
        _moveCost = 5;
        _contents = BlockContents.Water;
    }
}
