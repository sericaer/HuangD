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

    internal void SetCell((int x, int y) position, float value)
    {
        var color = CalcColor(value);

        tilemap.SetTileColor(new Vector3Int(position.x, position.y), tile, color);
    }

    private Color CalcColor(float value)
    {
        return rainMin + (rainMax - rainMin) * value;
    }
}