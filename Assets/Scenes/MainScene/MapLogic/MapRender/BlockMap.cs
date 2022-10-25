using Math.TileMap;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;

public class BlockMap : MapBehaviour
{

    private Dictionary<int, Color> dictColor;

    private Color GetColor(int key)
    {
        if (dictColor == null)
        {
            dictColor = new Dictionary<int, Color>();
        }

        if(dictColor.ContainsKey(key))
        {
            return dictColor[key];
        }

        while (true)
        {
            var color = Random.ColorHSV();
            if (!dictColor.ContainsValue(color))
            {
                dictColor.Add(key, color);
                return color;
            }
        }
    }

    internal void SetCell((int x, int y) position, int block, bool isBlockEdge)
    {
        var color = GetColor(block);

        if(isBlockEdge)
        {
            color.a = 0.3f;
        }

        tilemap.SetTileColor(new Vector3Int(position.x, position.y), tile, color);
    }
}
