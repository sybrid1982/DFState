using System;
using UnityEngine;

[System.Serializable]
public struct Point : IEquatable<Point> {
    public int x;
    public int y;
    public int z;

    public Point (int x, int y, int z)
    {
        this.x = x;
        this.y = y;
        this.z = z;
    }

    public static Point operator +(Point a, Point b)
    {
        return new Point(a.x + b.x, a.y + b.y, a.z + b.z);
    }

    public static Point operator -(Point a, Point b)
    {
        return new Point(a.x - b.x, a.y - b.y, a.z - b.z);
    }

    public static bool operator ==(Point a, Point b)
    {
        return a.x == b.x && a.y == b.y && a.z == b.z;
    }

    public static bool operator !=(Point a, Point b)
    {
        return !(a == b);
    }

    public override bool Equals(object obj)
    {
        if (obj is Point)
        {
            Point p = (Point)obj;
            return x == p.x && y == p.y && z == p.z;
        }
        return false;
    }

    public bool Equals (Point p)
    {
        return x == p.x && y == p.y && z == p.z;
    }

    public override int GetHashCode()
    {
        return x ^ y ^ z;
    }

    public override string ToString()
    {
        return string.Format("({0},{1},{2})", x, y,z);
    }

    public static implicit operator Vector3(Point p)
    {
        return new Vector3(p.x, p.y, p.z);
    }

    public static Point FromPosition (Vector3 position)
    {
        float x = position.x / SquareMetrics.sideLength;
        float y = position.y;
        float z = position.z / SquareMetrics.sideLength;
        int iX = Mathf.RoundToInt(x);
        int iY = Mathf.RoundToInt(y);
        int iZ = Mathf.RoundToInt(z);
        return new Point(iX, iY, iZ);
    }
}
