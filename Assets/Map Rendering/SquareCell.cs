using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SquareCell : MonoBehaviour {
    public Block block;

    public Point point;

    public Color Color {
        get { return GetColorFromBlockType(); }
    }

    public int GetElevation()
    {
        int hasBlock = 0;
        if (block.Type is BS_Solid)
            hasBlock = 1;
        return point.z + hasBlock;
    }
    
    [SerializeField]
    SquareCell[] neighbors;

    public SquareCell GetNeighbor (SquareDirection direction)
    {
        return neighbors[(int)direction];
    }

    public void SetNeighbor (SquareDirection direction, SquareCell cell)
    {
        neighbors[(int)direction] = cell;
        cell.neighbors[(int)direction.Opposite()] = this;
    }

    public void SetNeighbors(SquareGrid grid)
    {
        for(int i = 0; i < block.neighbors.Length; i++)
        {
            if(block.neighbors[i] != null)
            {
                SetNeighbor((SquareDirection)i, grid.GetSquareCellFromBlock(block.neighbors[i]));
            }
        }
    }

    public void RefreshPosition()
    {
        Vector3 position;
        position.x = point.x * SquareMetrics.sideLength;
        position.z = point.y * SquareMetrics.sideLength;
        position.y = GetElevation() * SquareMetrics.sideLength;
        transform.localPosition = position;
    }

    // Color should be moved to the BlockState class so that this
    // function just returns whatever the blockstate thinks the color should be
    // For water, that'll just be blue, but solid blocks will return colors
    // based on their material state, floors will return colors based on whether
    // they have grass or are roads or stone or whatever, etc

    // Leaving this for now, but this should be very different soon
    private Color GetColorFromBlockType()
    {
        if (block.Type is BS_Empty)
            return Color.green;
        else
            return block.Contents.color;
    }
}
