using System;
using UnityEngine;
using UnityEngine.Tilemaps;

public class CountryMap : MapBehaviour
{
    internal void SetCell(Vector3Int position, Color value)
    {
        tilemap.SetTileColor(position, tile, value);
    }

    internal void SetAlpha(float alpha)
    {
        tilemap.color = new Color(tilemap.color.r, tilemap.color.g, tilemap.color.b, alpha);
    }
}