using System;
using System.Collections;
using UnityEngine;

public class HeightMap : MapBehaviour
{
    internal void SetCell((int x, int y) pos, float value)
    {
        tilemap.SetTileColor(new Vector3Int(pos.x, pos.y), tile, new Color(value, value, value));
    }
}