using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathTile : Tile
{
    public Vector2Int NextToBaseTile;
    public int Distance;

    private void Start() 
    {
        SetColor();
    }
}
