using System;
using System.Collections;
using UnityEngine;

public class RainMap : MapBehaviour
{
    public  Color WetMax = Color.green;
    public  Color WetMin = Color.yellow;

    internal void SetCell(Vector3Int vector3Int, float value)
    {
        var color = CalcColor(value);

        tilemap.SetTileColor(vector3Int, tile, color);
    }

    private Color CalcColor(float value)
    {
        return WetMin + (WetMax - WetMin) * value;
    }
}