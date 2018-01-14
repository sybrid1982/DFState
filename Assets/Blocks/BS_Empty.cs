using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BS_Empty : BlockState {
    public BS_Empty()
    {
        _moveCost = 1;  // 1 will be the default cost
        _contents = BlockContents.Empty;
    }
}
