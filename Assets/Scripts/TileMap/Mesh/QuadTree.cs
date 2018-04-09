using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuadTree
{
    private static void SimplifyMap(float[,][,] heightData, bool[,][,] enabledVertices,
        Vector2Int lowerLeft, int range)
    {

    }
    /*
    public static void SimplifyMap(float[,][,] heightData)
    {
        int res = 16;
        bool[,][,] enabledVertices = new bool[heightData.GetLength(0), heightData.GetLength(1)][,];
        for (int i = 0; i < enabledVertices.GetLength(0); i++)
        {
            for (int j = 0; j < enabledVertices.GetLength(1); j++)
            {
                enabledVertices[i, j] = new bool[res, res];
            }
        }
        SimplifyMap(heightData, enabledVertices, new Vector2Int(0, 0), res);
    }*/
}
