using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RiverMap : MapBehaviour
{
    internal void SetCell(Vector3Int position)
    {
        tilemap.SetTileColor(position, tile, Color.blue);
    }
}
