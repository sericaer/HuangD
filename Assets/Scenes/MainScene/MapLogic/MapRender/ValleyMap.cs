using System;
using System.Collections.Generic;
using UnityEngine;

public class ValleyMap : MapBehaviour
{
    public HashSet<Color> colors;


    internal void SetValley(IEnumerable<(int x, int y)> valley)
    {
        var color = RandomColor();
        foreach(var elem in valley)
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
            var color = UnityEngine.Random.ColorHSV();
            if (!colors.Contains(color))
            {
                colors.Add(color);
                return color;
            }
        }
    }
}