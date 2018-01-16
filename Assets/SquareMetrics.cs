using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SquareMetrics {
    public const float sideLength = 1.5f;
    public const float innerRadius = sideLength / 2f;

    public const float solidFactor = 0.8f;
    public const float blendFactor = 1f - solidFactor;

    public const int chunkSizeX = 5, chunkSizeY = 5;

    static Vector3[] corners = {
        new Vector3(-innerRadius, 0f, -innerRadius),  // Bottom Left
        new Vector3(-innerRadius, 0f, innerRadius),   // Top Left
        new Vector3(innerRadius, 0f, innerRadius),    // Top Right
        new Vector3(innerRadius, 0f, -innerRadius)    // Bottom Right?
    };

    public static Vector3 GetFirstCorner(SquareDirection direction)
    {
        switch (direction) {
            case SquareDirection.N:
                return corners[1];
            case SquareDirection.E:
                return corners[2];
            case SquareDirection.S:
                return corners[3];
            case SquareDirection.W:
                return corners[0];
            default:
                return Vector3.zero;
        }
    }

    public static Vector3 GetSecondCorner(SquareDirection direction)
    {
        switch (direction)
        {
            case SquareDirection.N:
                return corners[2];
            case SquareDirection.E:
                return corners[3];
            case SquareDirection.S:
                return corners[0];
            case SquareDirection.W:
                return corners[1];
            default:
                return Vector3.zero;
        }
    }

    public static Vector3 GetFirstSolidCorner (SquareDirection direction)
    {
        return GetFirstCorner(direction) * solidFactor;
    }

    public static Vector3 GetSecondSolidCorner (SquareDirection direction)
    {
        return GetSecondCorner(direction) * solidFactor;
    }

    public static Vector3 GetBridge (SquareDirection direction)
    {
        return (GetFirstCorner(direction) + GetSecondCorner(direction)) * blendFactor;
    }
}
