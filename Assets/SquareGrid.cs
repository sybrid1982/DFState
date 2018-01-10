using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SquareGrid : MonoBehaviour {

    SquareCell[] cells;

    public SquareCell cellPrefab;

    SquareMesh squareMesh;

    public Color defaultColor = Color.white;
    public Color[] colors;
    public Color touchColor = Color.magenta;

    Dictionary<Block, SquareCell> blocksToCellsMap;

    int sizeX;
    int sizeY;

    public SquareCell GetSquareCellFromBlock(Block block)
    {
        if (blocksToCellsMap.ContainsKey(block))
            return blocksToCellsMap[block];
        else
            return null;
    }

    public void CreateMap(Block[,] blocks, int sizeX, int sizeY)
    {
        blocksToCellsMap = new Dictionary<Block, SquareCell>();
        this.sizeX = sizeX;
        this.sizeY = sizeY;

        squareMesh = GetComponentInChildren<SquareMesh>();

        cells = new SquareCell[sizeX * sizeY];

        for(int z = 0, i = 0; z < sizeY; z++)
        {
            for(int x=0; x < sizeX; x++)
            {
                CreateCell(x, z, i++, blocks[x, z]);
            }
        }
        SetNeighbors(cells);
        squareMesh.Triangulate(cells);
        InputHandler ih = FindObjectOfType<InputHandler>();
        ih.SetGrid(this);
    }

    void CreateCell(int x, int z, int i, Block block)
    {
        SquareCell cell = cells[i] = Instantiate(cellPrefab);
        cell.transform.SetParent(transform, false);
        cell.point = block.Point;
        cell.block = block;

        cell.RefreshPosition();

        blocksToCellsMap.Add(block, cell);
    }

    private void SetNeighbors(SquareCell[] cells)
    {
        for(int i = 0; i < cells.Length; i++)
        {
            cells[i].SetNeighbors(this);
        }
    }

    private void SetNeighbors(int x, int z, int i, SquareCell cell)
    {
        if (x > 0)
        {
            cell.SetNeighbor(SquareDirection.W, cells[i - 1]);
        }
        if (z > 0)
        {
            cell.SetNeighbor(SquareDirection.S, cells[i - sizeY]);
            if (x > 0)
            {
                cell.SetNeighbor(SquareDirection.SW, cells[i - sizeY - 1]);
            }
            if (x < sizeX)
            {
                cell.SetNeighbor(SquareDirection.SE, cells[i - sizeY + 1]);
            }
        }
    }

    public SquareCell GetCell(Vector3 position)
    {
        position = transform.InverseTransformPoint(position);
        Point point = Point.FromPosition(position);
        int index = point.x + point.z * sizeX;
        if (index > cells.Length)
            return null;

        return cells[index];
    }

    public void ChangeColor(int index)
    {
        touchColor = colors[index];
    }

    public void Refresh()
    {
        squareMesh.Triangulate(cells);
    }
}
