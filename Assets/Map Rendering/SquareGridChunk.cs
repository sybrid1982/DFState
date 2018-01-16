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

    public void AddCell(int index, SquareCell cell)
    {
        cells[index] = cell;
        cell.transform.SetParent(transform, false);
        cell.Chunk = this;
    }

    public void Refresh()
    {
        enabled = true;
    }

    private void LateUpdate()
    {
        squareMesh.Triangulate(cells);
        enabled = false;
    }
}
