using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BS_Empty : BlockState {
    public BS_Empty()
    {
        _moveCost = 1;  // 1 will be the default cost
        _contents = null;
        _health = 0;    // 0 health for empty blocks for now
                        // could instead be infinite health?
    }
}
