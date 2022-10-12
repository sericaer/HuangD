using System;
using System.Collections;
using UnityEngine;

public class RainMap : MapBehaviour
{
    public  Color rainMax = Color.green;
    public  Color rainMin = Color.yellow;

    internal void SetCell(Vector3Int vector3Int, float value)
    {
        var color = CalcColor(value);

        tilemap.SetTileColor(vector3Int, tile, color);
    }

    private Color CalcColor(float value)
    {
        return rainMin + (rainMax - rainMin) * value;
    }
}