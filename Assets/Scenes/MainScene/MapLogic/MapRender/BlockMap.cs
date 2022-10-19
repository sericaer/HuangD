using Math.TileMap;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;

public class BlockMap : MapBehaviour
{
    //private HashSet<Color> colors;

    private Dictionary<int, Color> dictColor;

    //internal void SetBlock(Block block)
    //{
    //    var color = RandomColor();

    //    foreach (var elem in block.elements)
    //    {
    //        tilemap.SetTileColor(new Vector3Int(elem.x, elem.y), tile, color);
    //    }
    //}

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

    internal void SetCell((int x, int y) position, int block)
    {
        var color = GetColor(block);

        tilemap.SetTileColor(new Vector3Int(position.x, position.y), tile, color);
    }
}
