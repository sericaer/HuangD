using HuangD.Interfaces;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainMap : MapBehaviour
{
    private Dictionary<TerrainType, Color> colors = new Dictionary<TerrainType, Color>()
    {
        { TerrainType.Plain, Color.green},
        { TerrainType.Hill, Color.yellow},
        { TerrainType.Mount, new Color(128 / 255f, 0, 128 / 255f)},
        { TerrainType.Water, Color.blue},
    };

    internal void SetCell(Vector3Int position, TerrainType value)
    {
        tilemap.SetTileColor(position, tile, colors[value]);
    }

    internal void SetCell((int x, int y) position, TerrainType value)
    {
        tilemap.SetTileColor(new Vector3Int(position.x, position.y), tile, colors[value]);
    }
}
