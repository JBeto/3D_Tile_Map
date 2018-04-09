using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatmullRomSplineTest : MonoBehaviour
{
    public bool RunBasicTests;
    public bool RunCustomTests;
    public List<Vector3> a;
    public int stepsPerCurve = 3;
    public int tension = 1;

    private void DrawCurve(List<Vector3> controlPoints, int steps, int t)
    {
        if (controlPoints.Count >= 2)
        {
            List<Vector3> points = CatmullRomSpline.GenerateSpline(controlPoints, steps, t);
            Vector3 previousPoint = points[0];
            for (int i = 1; i < points.Count; i++)
            {
                Gizmos.DrawLine(previousPoint, points[i]);
                previousPoint = points[i];
            }
        }
    }

    private void DrawCurve(List<Vector2> controlPoints, int steps, int t)
    {
        if (controlPoints.Count >= 2)
        {
            List<Vector2> points = CatmullRomSpline.GenerateSpline(controlPoints, steps, t);
            Vector3 previousPoint = points[0];
            for (int i = 1; i < points.Count; i++)
            {
                Gizmos.DrawLine(previousPoint, points[i]);
                previousPoint = points[i];
            }
        }
    }

    private void DrawCubic()
    {
        List<Vector2> cubic = new List<Vector2>() { new Vector2(-3, -3), Vector2.zero, new Vector2(3, 3) };
        DrawCurve(cubic, 3, 1);
    }

    private void OnDrawGizmos()
    {
        if (RunBasicTests)
        {
            DrawCubic();
        }
        if (RunCustomTests)
        {
            DrawCurve(a, stepsPerCurve, tension);
        }
    }
}
