using UnityEngine;
using System.Collections;

public class PerlinNoise
{
    public static float[,] GenerateNoiseMap(int mapWidth, int mapHeight, int octaves, float amp, float freq,
        int seed, Vector2 offset, float scale = 1f)
    {
		float[,] noiseMap = new float[mapWidth, mapHeight];
		System.Random prng = new System.Random(seed);
		Vector2[] octaveOffsets = new Vector2[octaves];

		for (int i = 0; i < octaves; i++) {
            float offsetX = prng.Next(-1000, 1000) + offset.x;
            float offsetY = prng.Next(-1000, 1000) + offset.y;
			octaveOffsets [i] = new Vector2 (offsetX, offsetY);
		}

		float maxNoiseHeight = float.MinValue;
		float minNoiseHeight = float.MaxValue;
        
        for (int x = 0; x < mapWidth; x++)
        {
            for (int y = 0; y < mapHeight; y++)
            {
				float amplitude = 1;
				float frequency = 1;
				float noiseHeight = 0;

                // Sum all octave maps
				for (int i = 0; i < octaves; i++) {
					float sampleX = (x / (mapWidth * scale * frequency)) + octaveOffsets[i].x;
					float sampleY = (y / (mapHeight * scale * frequency)) + octaveOffsets[i].y;

                    // convert to range [-1, 1] to account for negative amplitude
					float perlinValue = Mathf.PerlinNoise (sampleX, sampleY) * 2 - 1;

					noiseHeight += perlinValue * amplitude;
					amplitude *= amp;
					frequency *= freq;
				}

				if (noiseHeight > maxNoiseHeight) {
					maxNoiseHeight = noiseHeight;
				} else if (noiseHeight < minNoiseHeight) {
					minNoiseHeight = noiseHeight;
				}
                noiseMap[x, y] = noiseHeight;
			}
		}
		for (int x = 0; x < mapWidth; x++) {
			for (int y = 0; y < mapHeight; y++) {
				noiseMap[x, y] = Mathf.InverseLerp(minNoiseHeight, maxNoiseHeight, noiseMap[x, y]);
			}
		}
        
		return noiseMap;
	}
}