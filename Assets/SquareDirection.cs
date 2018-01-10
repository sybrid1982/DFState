using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SquareDirection {
    N, NE, E, SE, S, SW, W, NW
}

public static class SquareDirectionExtensions {
    public static SquareDirection Opposite (this SquareDirection direction)
    {
        return (int)direction < 4 ? (direction + 4) : (direction - 4);
    }

    public static SquareDirection Next (this SquareDirection direction)
    {
        return (int)direction < 7 ? (direction + 1) : (SquareDirection)0;
    }
    public static SquareDirection Previous(this SquareDirection direction)
    {
        return (int)direction > 0 ? (direction - 1) : (SquareDirection)6;
    }
    public static SquareDirection NextCardinal(this SquareDirection direction)
    {
        return (int)direction < 6 ? (direction + 2) : (SquareDirection)0;
    }
    public static SquareDirection PreviousCardinal(this SquareDirection direction)
    {
        return (int)direction > 1 ? (direction - 2) : (SquareDirection)6;
    }
}
