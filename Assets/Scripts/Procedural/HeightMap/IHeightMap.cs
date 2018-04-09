using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class IHeightMap : ScriptableObject
{
    public abstract float[,] GenerateHeightMap(int resolution);
}