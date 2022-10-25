using HuangD.Interfaces;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

public class ProvinceMap : MapBehaviour
{

    public Sprite[] provinceEgdes;

    private Tile[] _provinceEdgeTiles;

    private HashSet<Color> colors;

    private Color RandomColor()
    {
        if (colors == null)
        {
            colors = new HashSet<Color>();
        }

        while (true)
        {
            var color = Random.ColorHSV();
            if (!colors.Contains(color))
            {
                colors.Add(color);
                return color;
            }
        }
    }

    private Tile GetProvinceTile(int index)
    {
        if (_provinceEdgeTiles == null)
        {
            _provinceEdgeTiles = provinceEgdes.Select(x =>
            {
                var _tile = ScriptableObject.CreateInstance<Tile>();
                _tile.sprite = x;
                return _tile;
            }).ToArray();
        }

        return _provinceEdgeTiles[index];
    }

    internal void SetProvince(IProvince province)
    {
        var color = RandomColor();
        foreach (var cell in province.cells)
        {
            tilemap.SetTileColor(new Vector3Int(cell.position.x, cell.position.y), tile, color);
        }
    }
}