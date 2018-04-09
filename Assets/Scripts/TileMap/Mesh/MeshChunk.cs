using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public class MeshChunk
{
    public const int MaxSideLength = 3;

    /// <summary>
    /// - Takes in a list of height maps to create meshes for and 
    /// combines them together to form a SideLength x SideLength mesh chunk,
    /// which is attached to 'this' game object.
    /// 
    /// Accessed as follows:
    /// 2 5 8
    /// 1 4 7
    /// 0 3 6
    /// </summary>
    /// <param name="tileConfigs"></param>
    public static Mesh GenerateMeshChunk(float[,][,] tileConfigs, Color[,][,] colors, float tileSize)
    {
        if (tileConfigs.GetLength(0) > MaxSideLength || tileConfigs.GetLength(1) > MaxSideLength)
        {
            Debug.LogError(String.Format("Number of tiles exceed the max: {0} per mesh chunk", MaxSideLength));
        }
        Mesh chunk = new Mesh();
        CombineInstance[] tileInstances = new CombineInstance[tileConfigs.Length];
        for (int ci = 0, i = 0; i < tileConfigs.GetLength(0); i++)
        {
            for (int j = 0; j < tileConfigs.GetLength(1); j++, ci++)
            {
                CombineInstance meshTile = new CombineInstance()
                {
                    mesh = GenerateTile(tileConfigs[i, j], tileSize, colors[i, j]),
                    transform = Matrix4x4.Translate(new Vector3(i * tileSize, j * tileSize, 0))
                };
                tileInstances[ci] = meshTile;
            }
        }
        chunk.CombineMeshes(tileInstances);
        return chunk;
    }
    
    // 170 160 255
    public static Mesh GenerateTile(float[,] heightMap, float size, Color[,] colors)
    {
        int resolution = heightMap.GetLength(0);
        int triangleVerticeCount = (resolution - 1) * (resolution - 1) * 6;
        Vector3[] vertices = new Vector3[triangleVerticeCount];
        Color[] c = new Color[triangleVerticeCount];
        Vector3[] normals = new Vector3[triangleVerticeCount];
        int[] triangles = new int[triangleVerticeCount];
        
        for (int vertexIndex = 0, x = 0; x < resolution - 1; x++)
        {
            for (int y = 0; y < resolution - 1; y++, vertexIndex += 6)
            {
                // Position of each corner
                Vector3 lowerLeft = new Vector3(size * x / (resolution - 1), size * y / (resolution - 1), -heightMap[x, y]);
                Vector3 lowerRight = new Vector3(size * (x + 1) / (resolution - 1), size * y / (resolution - 1), -heightMap[x + 1, y]);
                Vector3 upperLeft = new Vector3(size * x / (resolution - 1), size * (y + 1) / (resolution - 1), -heightMap[x, y + 1]);
                Vector3 upperRight = new Vector3(size * (x + 1) / (resolution - 1), size * (y + 1) / (resolution - 1), -heightMap[x + 1, y + 1]);

                // Bottom left triangle
                vertices[vertexIndex] = lowerRight;
                vertices[vertexIndex + 1] = lowerLeft;
                vertices[vertexIndex + 2] = upperLeft;

                c[vertexIndex] = colors[x + 1, y];
                c[vertexIndex + 1] = colors[x, y];
                c[vertexIndex + 2] = colors[x, y + 1];

                Vector3 bottomLeftNormal = Vector3.Cross(lowerRight - lowerLeft, upperLeft - lowerLeft);
                normals[vertexIndex] = bottomLeftNormal;
                normals[vertexIndex + 1] = bottomLeftNormal;
                normals[vertexIndex + 2] = bottomLeftNormal;

                // Top right triangle
                vertices[vertexIndex + 3] = lowerRight;
                vertices[vertexIndex + 4] = upperLeft;
                vertices[vertexIndex + 5] = upperRight;

                c[vertexIndex + 3] = colors[x + 1, y];
                c[vertexIndex + 4] = colors[x, y + 1];
                c[vertexIndex + 5] = colors[x + 1, y + 1];

                Vector3 topRightNormal = Vector3.Cross(upperLeft - upperRight, lowerRight - upperRight);
                normals[vertexIndex + 3] = topRightNormal;
                normals[vertexIndex + 4] = topRightNormal;
                normals[vertexIndex + 5] = topRightNormal;

                // Set triangles
                for (int i = 0; i < 6; i++)
                {
                    triangles[vertexIndex + i] = vertexIndex + i;
                }
            }
        }

        // Creates mesh
        return new Mesh()
        {
            vertices = vertices,
            triangles = triangles,
            normals = normals,
            colors = c
        };
    }
}