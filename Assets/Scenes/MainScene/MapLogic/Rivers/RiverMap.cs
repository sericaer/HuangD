using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

public class RiverMap : MonoBehaviour
{
    public Tilemap tilemap;

    public Sprite[] riverSprites;

    private Tile[] _riverTiles;

    private Tile GetRiverTile(int index)
    {
        if (_riverTiles == null)
        {
            _riverTiles = riverSprites.Select(x =>
            {
                var _tile = ScriptableObject.CreateInstance<Tile>();
                _tile.sprite = x;
                return _tile;
            }).ToArray();
        }

        return _riverTiles[index];
    }

    internal void SetCell(Vector3Int position, int direct)
    {
        tilemap.SetTile(position, GetRiverTile(direct));
    }
}
