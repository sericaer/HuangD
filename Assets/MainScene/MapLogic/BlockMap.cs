using System;
using UnityEngine;

public class BlockMap : MapBehaviour
{
    internal void SetCell(Vector3Int position, Color value)
    {
        tilemap.SetTileColor(position, tile, value);
    }
}
