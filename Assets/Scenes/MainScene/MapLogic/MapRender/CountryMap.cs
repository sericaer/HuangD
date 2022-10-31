using HuangD.Interfaces;
using System;
using System.Linq;
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

    internal float GetAlpha()
    {
        return tilemap.color.a;
    }

    internal void SetCountry(ICountry country)
    {
        var color = new Color(country.color.r/255f, country.color.g/255f, country.color.b/255f);

        foreach (var cell in country.provinces.SelectMany(x=>x.cells))
        {
            tilemap.SetTileColor(new Vector3Int(cell.position.x, cell.position.y), tile, color);
        }
    }
}