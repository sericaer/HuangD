using Math.TileMap;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

public class EdgeMap : MonoBehaviour
{
    public Tilemap tileMap;

    public Sprite[] egdes;

    private Tile[] _tiles;

    internal void SetCell(Vector3Int pos, int value)
    {
        tileMap.SetTileColor(pos, GetTile(value), Color.white);
    }

    private Tile GetTile(int index)
    {
        if (_tiles == null)
        {
            _tiles = egdes.Select(x =>
            {
                var _tile = ScriptableObject.CreateInstance<Tile>();
                _tile.sprite = x;
                return _tile;
            }).ToArray();
        }

        return _tiles[index];
    }

}
