using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(TestTile))]
public class TestTileEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        TestTile t = (TestTile)target;
        if (GUILayout.Button("Make Tile"))
        {
            // t.GenerateTile();
        }
    }
}
