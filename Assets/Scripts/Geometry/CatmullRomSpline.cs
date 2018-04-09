using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Author: Andrew Hung
// Credit: https://andrewhungblog.wordpress.com/2017/03/03/catmull-rom-splines-in-plain-english/

public class CatmullRomSpline
{
    public static List<Vector2> GenerateSpline(List<Vector2> points, int stepsPerCurve = 3, float tension = 1)
    {
        List<Vector2> result = new List<Vector2>();

        for (int i = 0; i < points.Count - 1; i++)
        {
            Vector2 prev = i == 0 ? points[i] : points[i - 1];
            Vector2 currStart = points[i];
            Vector2 currEnd = points[i + 1];
            Vector2 next = i == points.Count - 2 ? points[i + 1] : points[i + 2];

            for (int step = 0; step <= stepsPerCurve; step++)
            {
                float t = (float)step / stepsPerCurve;
                float tSquared = t * t;
                float tCubed = tSquared * t;

                Vector2 interpolatedPoint =
                    (-.5f * tension * tCubed + tension * tSquared - .5f * tension * t) * prev +
                    (1 + .5f * tSquared * (tension - 6) + .5f * tCubed * (4 - tension)) * currStart +
                    (.5f * tCubed * (tension - 4) + .5f * tension * t - (tension - 3) * tSquared) * currEnd +
                    (-.5f * tension * tSquared + .5f * tension * tCubed) * next;

                result.Add(interpolatedPoint);
            }
        }

        return result;
    }

    public static List<Vector3> GenerateSpline(List<Vector3> points, int stepsPerCurve = 3, float tension = 1)
    {
        List<Vector3> result = new List<Vector3>();

        for (int i = 0; i < points.Count - 1; i++)
        {
            Vector3 prev = i == 0 ? points[i] : points[i - 1];
            Vector3 currStart = points[i];
            Vector3 currEnd = points[i + 1];
            Vector3 next = i == points.Count - 2 ? points[i + 1] : points[i + 2];

            for (int step = 0; step <= stepsPerCurve; step++)
            {
                float t = (float)step / stepsPerCurve;
                float tSquared = t * t;
                float tCubed = tSquared * t;

                Vector3 interpolatedPoint =
                    (-.5f * tension * tCubed + tension * tSquared - .5f * tension * t) * prev +
                    (1 + .5f * tSquared * (tension - 6) + .5f * tCubed * (4 - tension)) * currStart +
                    (.5f * tCubed * (tension - 4) + .5f * tension * t - (tension - 3) * tSquared) * currEnd +
                    (-.5f * tension * tSquared + .5f * tension * tCubed) * next;

                result.Add(interpolatedPoint);
            }
        }

        return result;
    }
}