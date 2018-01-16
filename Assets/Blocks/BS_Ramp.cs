using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BS_Ramp : BlockState {

    public BS_Ramp(BlockContents contents)
    {
        _moveCost = 2;
        _contents = contents;
    }
}
