using UnityEngine;
using UnityEngine.Tilemaps;
public static class MapLogicExtension
{
    public static void SetTileColor(this Tilemap map, Vector3Int pos, Tile tile, Color color)
    {
        map.SetTile(pos, tile);
        map.SetTileFlags(pos, TileFlags.None);
        map.SetColor(pos, color);
    }
}