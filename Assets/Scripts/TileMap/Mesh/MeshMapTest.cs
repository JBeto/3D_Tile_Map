using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class MeshMapTest : MonoBehaviour
{
    [SerializeField]
    private TileConfig land;
    [SerializeField]
    private TileConfig mountain;

    public void GenerateMap()
    {
        TileConfig[,] tileConfigs = new TileConfig[,] {
            { land, mountain, land },
            { land, mountain, land },
            { mountain, mountain, land }
        };
        MeshMap.GenerateMap(transform, tileConfigs);
    }
}

[CustomEditor(typeof(MeshMapTest))]
public class MeshMapTestEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        MeshMapTest meshMap = (MeshMapTest)target;
        if (GUILayout.Button("Generate Map"))
        {
            meshMap.GenerateMap();
        }
    }
}