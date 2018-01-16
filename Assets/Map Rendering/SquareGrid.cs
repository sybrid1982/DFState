using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SquareGrid : MonoBehaviour {

    SquareCell[] cells;

    public SquareCell cellPrefab;
    public SquareGridChunk squareGridChunk;

    public Color defaultColor = Color.white;
    public Color[] colors;
    public Color touchColor = Color.magenta;

    Dictionary<Block, SquareCell> blocksToCellsMap;
    SquareGridChunk[] chunks;

    int sizeX;
    int sizeY;

    int chunkCountX;
    int chunkCountY;

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

        CreateChunks();
        CreateCells(blocks, sizeX, sizeY);

        InputHandler ih = FindObjectOfType<InputHandler>();
        ih.SetGrid(this);
    }

    private void CreateCells(Block[,] blocks, int sizeX, int sizeY)
    {
        cells = new SquareCell[sizeX * sizeY];

        for (int z = 0, i = 0; z < sizeY; z++)
        {
            for (int x = 0; x < sizeX; x++)
            {
                CreateCell(x, z, i++, blocks[x, z]);
            }
        }
        SetNeighbors(cells);
    }

    void CreateChunks()
    {
        chunkCountX = sizeX / SquareMetrics.chunkSizeX;
        chunkCountY = sizeY / SquareMetrics.chunkSizeY;
        chunks = new SquareGridChunk[chunkCountX * chunkCountY];

        for(int z = 0, i = 0; z < chunkCountY; z++)
        {
            for(int x = 0; x < chunkCountX; x++)
            {
                SquareGridChunk chunk = chunks[i++] = Instantiate(squareGridChunk);
                chunk.transform.SetParent(transform);
            }
        }
    }

    void CreateCell(int x, int z, int i, Block block)
    {
        SquareCell cell = cells[i] = Instantiate(cellPrefab);

        cell.point = block.Point;
        cell.block = block;

        cell.RefreshPosition();

        blocksToCellsMap.Add(block, cell);

        AddCellToChunk(x, z, cell);
    }

    void AddCellToChunk(int x, int z, SquareCell cell)
    {
        int chunkX = x / SquareMetrics.chunkSizeX;
        int chunkZ = z / SquareMetrics.chunkSizeY;
        SquareGridChunk chunk = chunks[chunkX + chunkZ * chunkCountX];

        int localX = x - chunkX * SquareMetrics.chunkSizeY;
        int localZ = z - chunkZ * SquareMetrics.chunkSizeY;
        chunk.AddCell(localX + localZ * SquareMetrics.chunkSizeX, cell);
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
}
