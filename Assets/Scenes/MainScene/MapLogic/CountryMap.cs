using UnityEngine;

public class CountryMap : MapBehaviour
{
    internal void SetCell(Vector3Int position, Color value)
    {
        tilemap.SetTileColor(position, tile, value);
    }
}