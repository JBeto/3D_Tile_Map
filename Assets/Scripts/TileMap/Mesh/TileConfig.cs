using UnityEngine;

[CreateAssetMenu()]
public class TileConfig : ScriptableObject
{
    public Color tileColor;
    public IHeightMap heightMap;

    public float[,] GenerateHeightMap(int resolution)
    {
        return heightMap.GenerateHeightMap(resolution);
    }
}