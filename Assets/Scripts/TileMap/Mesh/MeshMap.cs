using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class MeshMap
{
    // How many vertices we use per tile side length
    private const int resolution = 16;
    // The size of each tile
    private const float tileSize = 10f;
    private const int blendWidth = 4;
    /// <summary>
    /// Returns the generated height maps from the 'tiles' parameter
    /// </summary>
    /// <param name="tiles"></param>
    /// <returns></returns>
    private static float[,][,] GenerateHeightMaps(TileConfig[,] tiles)
    {
        float[,][,] tileHeightMaps = new float[tiles.GetLength(0), tiles.GetLength(1)][,];
        for (int x = 0; x < tiles.GetLength(0); x++)
        {
            for (int y = 0; y < tiles.GetLength(1); y++)
            {
                tileHeightMaps[x, y] = tiles[x, y].GenerateHeightMap(resolution);
            }
        }
        return tileHeightMaps;
    }

    private static Color[,] GenerateColors(TileConfig[,] tiles)
    {
        Color[,] colors = new Color[tiles.GetLength(0), tiles.GetLength(1)];
        for (int x = 0; x < tiles.GetLength(0); x++)
        {
            for (int y = 0; y < tiles.GetLength(1); y++)
            {
                colors[x, y] = tiles[x, y].tileColor;
            }
        }
        return colors;
    }

    private static GameObject CreateEmptyMesh()
    {
        GameObject emptyMesh = new GameObject();
        emptyMesh.AddComponent<MeshRenderer>();
        emptyMesh.AddComponent<MeshFilter>();
        emptyMesh.AddComponent<MeshCollider>();
        return emptyMesh;
    }

    private static void GenerateMeshChunks(float[,][,] heightMaps, Color[,][,] c, Transform map)
    {
        GameObject chunkPrefab = CreateEmptyMesh();
        for (int x = 0; x < heightMaps.GetLength(0); x += MeshChunk.MaxSideLength)
        {
            for (int y = 0; y < heightMaps.GetLength(1); y += MeshChunk.MaxSideLength)
            {
                int chunkWidth = (x + MeshChunk.MaxSideLength <= heightMaps.GetLength(0))
                    ? MeshChunk.MaxSideLength : heightMaps.GetLength(0) - x;
                int chunkHeight = (y + MeshChunk.MaxSideLength <= heightMaps.GetLength(1))
                    ? MeshChunk.MaxSideLength : heightMaps.GetLength(1) - y;
                float[,][,] chunk = new float[chunkWidth, chunkHeight][,];
                Color[,][,] colors = new Color[chunkWidth, chunkHeight][,];
                for (int xi = 0; xi < chunkWidth; xi++)
                {
                    for (int yi = 0; yi < chunkHeight; yi++)
                    {
                        chunk[xi, yi] = heightMaps[x + xi, y + yi];
                        colors[xi, yi] = c[x + xi, y + yi];
                    }
                }

                GameObject mapChunk = Object.Instantiate(chunkPrefab, map);
                mapChunk.GetComponent<MeshFilter>().mesh = MeshChunk.GenerateMeshChunk(chunk, colors, tileSize);
                mapChunk.transform.localPosition = new Vector3(x * tileSize, y * tileSize, 0);
            }
        }
        Object.DestroyImmediate(chunkPrefab);
    }
    
    /// <summary>
    /// Generates the map corresponding to the 'tileConfigs' and attaches them to 'map'
    /// IMPORTANT NOTE: 'tileConfigs' is referenced as [x, y], the cartesian coordinates, NOT [row, col]
    /// </summary>
    /// <param name="map"></param>
    public static void GenerateMap(Transform map, TileConfig[,] tileConfigs)
    {
        float[,][,] tileHeightMaps = GenerateHeightMaps(tileConfigs);
        Color[,][,] colors = new Color[tileConfigs.GetLength(0), tileConfigs.GetLength(1)][,];
        for (int i = 0; i < tileConfigs.GetLength(0); i++)
        {
            for (int j = 0; j < tileConfigs.GetLength(1); j++)
            {
                colors[i, j] = new Color[resolution, resolution];
                Color[,] currentTile = colors[i, j];
                for (int a = 0; a < resolution; a++)
                {
                    for (int b = 0; b < resolution; b++)
                    {
                        currentTile[a, b] = tileConfigs[i, j].tileColor;
                    }
                }
            }
        }
        ConnectHeightMaps(tileHeightMaps, colors);
        GenerateMeshChunks(tileHeightMaps, colors, map);
    }
    
    private static void ConnectHeightMaps(float[,][,] tiles, Color[,][,] colors)
    {
        // Vertical connections
        for(int x = 0; x < tiles.GetLength(0) - 1; x++)
        {
            for (int y = 0; y < tiles.GetLength(1); y++)
            {
                float[,] leftTile = tiles[x, y];
                float[,] rightTile = tiles[x + 1, y];

                Color[,] leftTileColor = colors[x, y];
                Color[,] rightTileColor = colors[x + 1, y];

                for (int yi = 0; yi < resolution; yi++)
                {
                    float avg = Mathf.Lerp(leftTile[resolution - 1, yi], rightTile[0, yi], 0.5f);
                    leftTile[resolution - 1, yi] = avg;
                    rightTile[0, yi] = avg;

                    Color avgColor = Color.Lerp(leftTileColor[resolution - 1, yi], rightTileColor[0, yi], 0.5f);
                    leftTileColor[resolution - 1, yi] = avgColor;
                    rightTileColor[0, yi] = avgColor;

                }
                // Blend portion
                for (int i = 1; i < blendWidth; i++)
                {
                    float blendWeight = i / (float)blendWidth;
                    
                    for (int t = 0; t < resolution; t++)
                    {
                        float blendedValue = leftTile[resolution - 1, t];
                        leftTile[resolution - 1 - i, t] = Mathf.Lerp(blendedValue, leftTile[resolution - 1 - i, t], blendWeight);
                        rightTile[i, t] = Mathf.Lerp(blendedValue, rightTile[i, t], blendWeight);

                        Color blendedColor = leftTileColor[resolution - 1, t];
                        leftTileColor[resolution - 1 - i, t] = Color.Lerp(blendedColor, leftTileColor[resolution - 1 - i, t], blendWeight);
                        rightTileColor[i, t] = Color.Lerp(blendedColor, rightTileColor[i, t], blendWeight);
                    }
                }
            }
        }
        // Horizontal connections
        for (int y = 0; y < tiles.GetLength(1) - 1; y++)
        {
            for (int x = 0; x < tiles.GetLength(0); x++)
            {
                float[,] topTile = tiles[x, y + 1];
                float[,] bottomTile = tiles[x, y];

                Color[,] topTileColor = colors[x, y + 1];
                Color[,] bottomTileColor = colors[x, y];

                for (int xi = 0; xi < resolution; xi++)
                {
                    float avg = Mathf.Lerp(topTile[xi, 0], bottomTile[xi, resolution - 1], 0.5f);
                    topTile[xi, 0] = avg;
                    bottomTile[xi, resolution - 1] = avg;

                    Color avgColor = Color.Lerp(topTileColor[xi, 0], bottomTileColor[xi, resolution - 1], 0.5f);
                    topTileColor[xi, 0] = avgColor;
                    bottomTileColor[xi, resolution - 1] = avgColor;
                }
                // Blend portion
                for (int i = 1; i < blendWidth; i++)
                {
                    float blendWeight = i / (float)blendWidth;
                    for (int t = 0; t < resolution; t++)
                    {
                        float blendedValue = bottomTile[t, resolution - 1];
                        bottomTile[t, resolution - 1 - i] = Mathf.Lerp(blendedValue, bottomTile[t, resolution - 1 - i], blendWeight);
                        topTile[t, i] = Mathf.Lerp(blendedValue, topTile[t, i], blendWeight);

                        Color blendedColor = bottomTileColor[t, resolution - 1];
                        bottomTileColor[t, resolution - 1 - i] = Color.Lerp(blendedColor, bottomTileColor[t, resolution - 1 - i], blendWeight);
                        topTileColor[t, i] = Color.Lerp(blendedColor, topTileColor[t, i], blendWeight);
                    }
                }
            }
        }
    }
}