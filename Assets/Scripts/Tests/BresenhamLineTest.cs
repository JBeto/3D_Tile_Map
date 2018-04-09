using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BresenhamLineTest : MonoBehaviour
{
    public bool RunBasicTests;
    public bool RunCustomTests;
    public List<Vector2Int> a;
    public List<Vector2Int> b;

    private List<Vector3> CustomLine(Vector2Int a, Vector2Int b)
    {
        var gizmoPoints = new List<Vector3>();
        var points = BresenhamLine.PlotLine(a, b);
        foreach (var p in points)
        {
            gizmoPoints.Add(new Vector3(p.x, p.y, 0));
        }
        return gizmoPoints;
    }

    private List<Vector3> HorizontalLine()
    {
        return CustomLine(new Vector2Int(1, 0), new Vector2Int(5, 0));
    }

    private List<Vector3> DiagonalLine()
    {
        return CustomLine(new Vector2Int(1, 1), new Vector2Int(3, 3));
    }

    private List<Vector3> VerticalLine()
    {
        return CustomLine(new Vector2Int(0, 1), new Vector2Int(0, 5));
    }

    public void OnDrawGizmos()
    {
        if (RunBasicTests)
        {
            foreach(Vector3 v in VerticalLine())
            {
                Gizmos.DrawCube(v, Vector3.one * 0.25f);
            }
            foreach (Vector3 v in HorizontalLine())
            {
                Gizmos.DrawCube(v, Vector3.one * 0.25f);
            }
            foreach (Vector3 v in DiagonalLine())
            {
                Gizmos.DrawCube(v, Vector3.one * 0.25f);
            }
        }
        if (RunCustomTests)
        {
            if (a.Count != b.Count)
            {
                Debug.LogError("Number of points from list 'A' does not match the number of points in list 'B'");
            }
            for(int i = 0; i < a.Count; i++)
            {
                foreach (Vector3 v in CustomLine(a[i], b[i]))
                {
                    Gizmos.DrawCube(v, Vector3.one * 0.25f);
                }
            }
        }
    }
}