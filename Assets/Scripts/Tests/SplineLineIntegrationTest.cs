using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplineLineIntegrationTest : MonoBehaviour
{
    public bool RunBasicTests;
    public bool RunCustomTests;
    public List<Vector3> a;
    public int stepsPerCurve = 3;
    public int tension = 1;
    
    private Vector2Int ToVector2Int(Vector3 a)
    {
        return new Vector2Int(Mathf.RoundToInt(a.x), Mathf.RoundToInt(a.y));
    }

    private Vector3 ToVector3(Vector2Int a)
    {
        return new Vector3(a.x, a.y, 0);
    }

    private void DrawCurve(List<Vector3> controlPoints, int steps, int t)
    {
        if (controlPoints.Count >= 2)
        {
            List<Vector3> points = CatmullRomSpline.GenerateSpline(controlPoints, steps, t);
            Vector3 previousPoint = points[0];
            for (int i = 1; i < points.Count; i++)
            {
                var rasterization = BresenhamLine.PlotLine(ToVector2Int(previousPoint), ToVector2Int(points[i]));
                foreach(var p in rasterization)
                {
                    Gizmos.DrawCube(ToVector3(p), Vector3.one * 0.2f);
                }
                previousPoint = points[i];
            }
        }
    }

    private void OnDrawGizmos()
    {
        if (RunCustomTests)
        {
            DrawCurve(a, stepsPerCurve, tension);
        }
    }
}
