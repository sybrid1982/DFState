using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SquareGridChunk : MonoBehaviour {
    SquareCell[] cells;

    SquareMesh squareMesh;

    private void Awake()
    {
        squareMesh = GetComponentInChildren<SquareMesh>();

        cells = new SquareCell[SquareMetrics.chunkSizeX * SquareMetrics.chunkSizeY];
    }

    private void Start()
    {
        if(cells.Length > 0)
            squareMesh.Triangulate(cells);
    }

    public void AddCell(int index, SquareCell cell)
    {
        cells[index] = cell;
        cell.transform.SetParent(transform, false);
    }

    public void Refresh()
    {
        squareMesh.Triangulate(cells);
    }
}
