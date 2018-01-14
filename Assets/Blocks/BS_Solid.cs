using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BS_Solid : BlockState {
    public BS_Solid(BlockContents contents)
    {
        _moveCost = 0;  // Move Cost of 0 => IMPASSABLE
        _contents = contents;
    }
}
