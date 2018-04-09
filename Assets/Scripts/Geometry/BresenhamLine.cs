using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BresenhamLine
{
    private static List<Vector2Int> PlotLineLow(int x0, int y0, int x1, int y1)
    {
        int dx = x1 - x0;
        int dy = y1 - y0;
        int yi = 1;
        if (dy < 0)
        {
            yi = -1;
            dy = -dy;
        }
        int D = 2 * dy - dx;
        int y = y0;

        List<Vector2Int> points = new List<Vector2Int>();
        for (int x = x0; x <= x1; x++)
        {
            points.Add(new Vector2Int(x, y));
            if (D > 0)
            {
                y = y + yi;
                D = D - 2 * dx;
            }
            D += 2 * dy;
        }
        return points;
    }

    private static List<Vector2Int> PlotLineHigh(int x0, int y0, int x1, int y1)
    {
        int dx = x1 - x0;
        int dy = y1 - y0;
        int xi = 1;
        if (dx < 0)
        {
            xi = -1;
            dx = -dx;
        }
        int D = 2 * dx - dy;
        int x = x0;

        List<Vector2Int> points = new List<Vector2Int>();
        for (int y = y0; y <= y1; y++)
        {
            points.Add(new Vector2Int(x, y));
            if (D > 0)
            {
                x = x + xi;
                D = D - 2 * dy;
            }
            D += 2 * dx;
        }
        return points;
    }

    public static List<Vector2Int> PlotLine(Vector2Int p0, Vector2Int p1)
    {
        if (Mathf.Abs(p1.y - p0.y) < Mathf.Abs(p1.x - p0.x))
        {
            if (p0.x > p1.x)
            {
                return PlotLineLow(p1.x, p1.y, p0.x, p0.y);
            }
            else
            {
                return PlotLineLow(p0.x, p0.y, p1.x, p1.y);
            }
        }
        else
        {
            if (p0.y > p1.y)
            {
                return PlotLineHigh(p1.x, p1.y, p0.x, p0.y);
            }
            else
            {
                return PlotLineHigh(p0.x, p0.y, p1.x, p1.y);
            }
        }
    }

    public static List<Vector2Int> PlotLine(int x0, int y0, int x1, int y1)
    {
        if (Mathf.Abs(y1 - y0) < Mathf.Abs(x1 - x0))
        {
            if (x0 > x1)
            {
                return PlotLineLow(x1, y1, x0, y0);
            }
            else
            {
                return PlotLineLow(x0, y0, x1, y1);
            }
        }
        else
        {
            if (y0 > y1)
            {
                return PlotLineHigh(x1, y1, x0, y0);
            }
            else
            {
                return PlotLineHigh(x0, y0, x1, y1);
            }
        }
    }
}