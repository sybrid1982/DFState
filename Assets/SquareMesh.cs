using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class SquareMesh : MonoBehaviour {
    Mesh squareMesh;
    List<Vector3> vertices;
    List<int> triangles;
    List<Color> colors;
    

    MeshCollider meshCollider;

    private void Awake()
    {
        GetComponent<MeshFilter>().mesh = squareMesh = new Mesh();
        meshCollider = gameObject.AddComponent<MeshCollider>();
        squareMesh.name = "Square Mesh";
        vertices = new List<Vector3>();
        colors = new List<Color>();
        triangles = new List<int>();
    }

    public void Triangulate(SquareCell[] cells)
    {
        squareMesh.Clear();
        vertices.Clear();
        colors.Clear();
        triangles.Clear();

        for (int i = 0; i < cells.Length; i++)
        {
            Triangulate(cells[i]);
        }
        squareMesh.vertices = vertices.ToArray();
        squareMesh.colors = colors.ToArray();
        squareMesh.triangles = triangles.ToArray();
        squareMesh.RecalculateNormals();
        meshCollider.sharedMesh = squareMesh;
    }

    
    void Triangulate (SquareCell cell)
    {
        
        for (SquareDirection d = SquareDirection.N; d < SquareDirection.NW; d++)
        {
            // Debug.Log("Triangulating direction " + d.ToString());
            Triangulate(d, cell);
            d++;
        }
    }

    void Triangulate(SquareDirection direction, SquareCell cell)
    {
        int zLevel = Camera.main.GetComponent<CameraController>().DisplayZ;
        cell.RefreshPosition();
        Vector3 center = cell.transform.localPosition;
        Vector3 v1 = center + SquareMetrics.GetFirstSolidCorner(direction);
        Vector3 v2 = center + SquareMetrics.GetSecondSolidCorner(direction);
        AddTriangle(center, v1, v2);
        AddTriangleColor(cell.Color);
        if (direction <= SquareDirection.E)
        {
            BuildBridge(direction, cell, v1, v2);
        }
        // From now on, we will only fill the hole from one direction
        // North, because it's the first
        if (direction == SquareDirection.N)
            FillHole(cell, v2);
    }

    private void BuildBridge(SquareDirection direction, SquareCell cell, Vector3 v1, Vector3 v2)
    {
        SquareCell neighbor = cell.GetNeighbor(direction);
        if (neighbor == null)
            return;
        Vector3 bridge = SquareMetrics.GetBridge(direction);
        Vector3 v3 = v1 + bridge;
        Vector3 v4 = v2 + bridge;
        v3.y = v4.y = neighbor.transform.localPosition.y;

        AddQuad(v1, v2, v3, v4);
        AddQuadColor(cell.Color, neighbor.Color);
    }

    void FillHole(SquareCell cell, Vector3 v1)
    {
        SquareCell eastNeighbor = cell.GetNeighbor(SquareDirection.E);
        SquareCell northNeighbor = cell.GetNeighbor(SquareDirection.N);
        SquareCell northEastNeighbor = null;
        if (northNeighbor == null || eastNeighbor == null)
            return;

        northEastNeighbor = cell.GetNeighbor(SquareDirection.N).GetNeighbor(SquareDirection.E);

        Vector3 bridgeN = SquareMetrics.GetBridge(SquareDirection.N);
        Vector3 n = v1 + bridgeN;

        Vector3 bridgeE = SquareMetrics.GetBridge(SquareDirection.E);
        Vector3 e = v1 + bridgeE;

        Vector3 ne = v1 + bridgeN + bridgeE;

        Color northColor, eastColor, northEastColor;
        northColor = eastColor = northEastColor = cell.Color;

        if (northNeighbor != null) {
            n.y = northNeighbor.transform.localPosition.y;
            northColor = northNeighbor.Color;
        }
        if (eastNeighbor != null) {
            e.y = eastNeighbor.transform.localPosition.y;
            eastColor = eastNeighbor.Color;
        }
        if (northEastNeighbor != null) {
            ne.y = northEastNeighbor.transform.localPosition.y;
            northEastColor = northEastNeighbor.Color;
        }

        Vector3 center = v1 + 0.5f * (bridgeN + bridgeE);
        center.y = (Mathf.Max(v1.y, e.y, n.y, ne.y) + Mathf.Min(v1.y, e.y, n.y, ne.y))/2f;
        Color blendColor = GetBlendColor(cell.Color, northColor, eastColor, northEastColor);

        // Triangle vertices should be clockwise
        // First Triangle to add will be v1, n, center
        AddTriangle(v1, n, center);
        AddTriangleColor(cell.Color, northColor, blendColor);
        // Second will be v1, center, e
        AddTriangle(v1, center, e);
        AddTriangleColor(cell.Color, blendColor, eastColor);
        // Third will be n, ne, center
        AddTriangle(n, ne, center);
        AddTriangleColor(northColor, northEastColor, blendColor);
        // Last is ne, e, center
        AddTriangle(ne, e, center);
        AddTriangleColor(northEastColor, eastColor, blendColor);
    }

    Color GetBlendColor(Color c1, Color c2, Color c3, Color c4)
    {
        float r = (c1.r + c2.r + c3.r + c4.r) / 4f;
        float g = (c1.g + c2.g + c3.g + c4.g) / 4f;
        float b = (c1.b + c2.b + c3.b + c4.b) / 4f;
        float a = (c1.a + c2.a + c3.a + c4.a) / 4f;

        return new Color(r, g, b, a);
    }

    void AddTriangle(Vector3 v1, Vector3 v2, Vector3 v3)
    {
        int vertexIndex = vertices.Count;
        vertices.Add(v1);
        vertices.Add(v2);
        vertices.Add(v3);
        triangles.Add(vertexIndex);
        triangles.Add(vertexIndex + 1);
        triangles.Add(vertexIndex + 2);
    }
    
    void AddTriangleColor(Color color)
    {
        colors.Add(color);
        colors.Add(color);
        colors.Add(color);
    }

    void AddTriangleColor(Color c1, Color c2, Color c3)
    {
        colors.Add(c1);
        colors.Add(c2);
        colors.Add(c3);
    }

    void AddQuad(Vector3 v1, Vector3 v2, Vector3 v3, Vector3 v4)
    {
        int vertexIndex = vertices.Count;
        vertices.Add(v1);
        vertices.Add(v2);
        vertices.Add(v3);
        vertices.Add(v4);
        triangles.Add(vertexIndex);
        triangles.Add(vertexIndex + 2);
        triangles.Add(vertexIndex + 1);
        triangles.Add(vertexIndex + 1);
        triangles.Add(vertexIndex + 2);
        triangles.Add(vertexIndex + 3);
    }

    void AddQuadColor(Color c1, Color c2, Color c3, Color c4)
    {
        colors.Add(c1);
        colors.Add(c2);
        colors.Add(c3);
        colors.Add(c4);
    }

    void AddQuadColor(Color c1, Color c2)
    {
        AddQuadColor(c1, c1, c2, c2);
    }
}
