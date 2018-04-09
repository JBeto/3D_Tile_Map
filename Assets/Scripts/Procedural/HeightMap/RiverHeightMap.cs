using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[CreateAssetMenu(fileName = "River", menuName = "Height Maps/River", order = 2)]
public class RiverHeightMap : IHeightMap
{
    [SerializeField, Range(0.5f, 0.80f)]
    private float riverWidthPercent;
    
    [SerializeField]
    private float landHeight;

    [SerializeField]
    private float riverDepth;

    [SerializeField]
    private AnimationCurve blendShore;

    public override float[,] GenerateHeightMap(int resolution)
    {
        float[,] heightMap = GenerateLand(resolution);
        GenerateRiverBed(heightMap);
        return heightMap;
    }

    // returns a list of control points
    private List<Vector2> GenerateControlPoints(int riverLength, int riverPerturbation, int numberOfBends)
    {
        List<Vector2> bendPoints = new List<Vector2>(2 + numberOfBends) { new Vector2(0, 0) };
        bool isRightBend = Random.Range(0, 2) == 0 ? false : true;
        Debug.Log("River length: " + riverLength);
        
        int bendDistance = riverLength / (numberOfBends + 1);
        Debug.Log("Bend Distance: " + bendDistance);

        for (int curr = bendDistance, i = 0; i < numberOfBends; i++, curr += bendDistance)
        {
            int b = isRightBend ? riverPerturbation : -riverPerturbation;
            bendPoints.Add(new Vector2(b, curr));
            Debug.Log(new Vector2(b, curr));
            isRightBend = !isRightBend;
        }
        bendPoints.Add(new Vector2(0, riverLength - 1));
        return bendPoints;
    }

    private List<Vector2> AdjustControlPoints(List<Vector2> controlPoints, int offset)
    {
        List<Vector2> adjustedControlPoints = new List<Vector2>(controlPoints);
        for(int i = 0; i < adjustedControlPoints.Count; i++)
        {
            adjustedControlPoints[i] = new Vector2(adjustedControlPoints[i].x + offset,
                adjustedControlPoints[i].y);
        }
        return adjustedControlPoints;
    }

    private List<Vector2Int> ConnectRiverBends(List<Vector2> splineCurve)
    {
        List<Vector2Int> riverPoints = new List<Vector2Int>();
        Vector2Int previous = new Vector2Int(Mathf.RoundToInt(splineCurve[0].x),
            Mathf.RoundToInt(splineCurve[0].y));
        for (int i = 1; i < splineCurve.Count; i++)
        {
            Vector2Int current = new Vector2Int(Mathf.RoundToInt(splineCurve[i].x),
                Mathf.RoundToInt(splineCurve[i].y));
            foreach(Vector2Int point in BresenhamLine.PlotLine(previous, current))
            {
                riverPoints.Add(point);
            }
            previous = current;
        }
        return riverPoints;
    }
    
    private void GenerateRiverBed(float[,] heightMap)
    {
        int resolution = heightMap.GetLength(0);
        // River width
        int riverWidth = (int)(resolution * riverWidthPercent) - 2;
        int maxRiverVariance = (int)(riverWidth * 0.17f);
        // Shore
        int shoreWidth = (int)(resolution * 0.1f);
        int maxShoreVariance = (int)(shoreWidth * 0.15f);
        // River indices
        int startOfRiver = (resolution - riverWidth) / 2;
        int endOfRiver = (resolution + riverWidth) / 2;
        // Generate control points
        List<Vector2> mainControlPoints = GenerateControlPoints(resolution, maxRiverVariance, 2);
        List<Vector2> leftControlPoints = AdjustControlPoints(mainControlPoints, startOfRiver);
        List<Vector2> rightControlPoints = AdjustControlPoints(mainControlPoints, endOfRiver);
        // Spline Curves
        List<Vector2> leftSplineCurve = CatmullRomSpline.GenerateSpline(leftControlPoints);
        List<Vector2> rightSplineCurve = CatmullRomSpline.GenerateSpline(rightControlPoints);
        // Interpolate rest of curve
        var leftRiver = ConnectRiverBends(leftSplineCurve);
        var rightRiver = ConnectRiverBends(rightSplineCurve);
        // Set left boundary
        foreach(Vector2Int leftBoundary in leftRiver)
        {
            heightMap[leftBoundary.x, leftBoundary.y] = riverDepth;
        }
        // Color left half of river, starting from center
        for (int y = 0; y < resolution; y++)
        {
            for (int i = resolution / 2; heightMap[i, y] != riverDepth; i--)
            {
                heightMap[i, y] = riverDepth;
            }
        }
        // Set right boundary
        foreach (Vector2Int rightBoundary in rightRiver)
        {
            heightMap[rightBoundary.x, rightBoundary.y] = riverDepth;
        }
        // Color until we hit right boundary, start from center
        for (int y = 0; y < resolution; y++)
        {
            for (int i = resolution / 2 + 1; heightMap[i, y] != riverDepth; i++)
            {
                heightMap[i, y] = riverDepth;
            }
        }
    }

    private float[,] GenerateLand(int resolution)
    {
        float[,] heightMap = PerlinNoise.GenerateNoiseMap(resolution, resolution,
            4, 0.4f, 0.6f, Random.Range(0, 10000), new Vector2(0, 0), 2f);
        LerpHeightMap(heightMap);
        return heightMap;
    }

    private void LerpHeightMap(float[,] heightMap)
    {
        for (int x = 0; x < heightMap.GetLength(0); x++)
        {
            for (int y = 0; y < heightMap.GetLength(1); y++)
            {
                heightMap[x, y] = Mathf.Lerp(0, 5, landHeight + heightMap[x, y]);
            }
        }
    }
}