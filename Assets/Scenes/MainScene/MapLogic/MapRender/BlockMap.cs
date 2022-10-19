using Math.TileMap;
using System.Collections.Generic;
using UnityEngine;

public class BlockMap : MapBehaviour
{
    private HashSet<Color> colors;

    internal void SetBlock(Block block)
    {
        var color = RandomColor();

        foreach (var elem in block.elements)
        {
            tilemap.SetTileColor(new Vector3Int(elem.x, elem.y), tile, color);
        }
    }

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
}
