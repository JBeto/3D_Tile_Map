using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Simple", menuName = "Height Maps/Simple", order = 1)]
public class PerlinHeightMap : IHeightMap
{
    [SerializeField]
    private float minHeight;

    [SerializeField]
    private float maxHeight;

    [SerializeField]
    private float scale;
    
    [SerializeField]
    private AnimationCurve interpolationCurve;

    [Range(1, 8)]
    [SerializeField]
    private int octaves;

    [Range(0.1f, 1)]
    [SerializeField]
    private float persistence;

    [Range(0.1f, 1)]
    [SerializeField]
    private float lacunarity;


    // Linearly interpolates height map from range of [0, 1] to a range of [MinHeight, MaxHeight]
    private void LerpHeightMap(float[,] heightMap)
    {
        float range = this.maxHeight - this.minHeight;
        for (int x = 0; x < heightMap.GetLength(0); x++)
        {
            for (int y = 0; y < heightMap.GetLength(1); y++)
            {
                heightMap[x, y] = this.minHeight + Mathf.Lerp(0, range, this.interpolationCurve.Evaluate(heightMap[x, y]));
            }
        }
    }

    public override float[,] GenerateHeightMap(int resolution)
    {
        float[,] heightMap = PerlinNoise.GenerateNoiseMap(resolution, resolution, 
            this.octaves, this.persistence, this.lacunarity, Random.Range(0, 10000), new Vector2(0, 0), scale);
        LerpHeightMap(heightMap);
        return heightMap;
    }
}