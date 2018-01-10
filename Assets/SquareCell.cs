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
        if (block.Type == BlockType.Solid)
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

    private Color GetColorFromBlockType()
    {
        if (block.Type == BlockType.Empty)
            return Color.green;
        if (block.Type == BlockType.Water)
            return Color.blue;
        else
        {
            if (block.Contents == BlockContents.Dirt)
                return new Color(0.4f, 0.4f, 0f, 1f);
            else
                return Color.gray;
        }
    }
}
