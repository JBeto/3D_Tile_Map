using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(MeshFilter))]
public class TestTile : MonoBehaviour
{
    public TileConfig terrain;

    public void GenerateTile()
    {
        // GetComponent<MeshFilter>().mesh = terrain.G
    }
}
