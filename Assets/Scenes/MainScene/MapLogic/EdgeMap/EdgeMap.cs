using Math.TileMap;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

public class EdgeMap : MonoBehaviour
{
    public enum EdgeType
    {
        Province,
        Country
    }

    public Tilemap tileMap;

    public Sprite[] provinceEgdes;

    public Sprite[] countryEgdes;

    private Tile[] _provinceEdgeTiles;
    private Tile[] _countryEdgeTiles;

    internal void SetCell(Vector3Int pos, int value, EdgeType edgeType)
    {
        switch(edgeType)
        {
            case EdgeType.Country:
                tileMap.SetTileColor(pos, GetCountryTile(value), Color.white);
                break;
            case EdgeType.Province:
                tileMap.SetTileColor(pos, GetProvinceTile(value), Color.white);
                break;
        }

    }

    private Tile GetProvinceTile(int index)
    {
        if (_provinceEdgeTiles == null)
        {
            _provinceEdgeTiles = provinceEgdes.Select(x =>
            {
                var _tile = ScriptableObject.CreateInstance<Tile>();
                _tile.sprite = x;
                return _tile;
            }).ToArray();
        }

        return _provinceEdgeTiles[index];
    }

    private Tile GetCountryTile(int index)
    {
        if (_countryEdgeTiles == null)
        {
            _countryEdgeTiles = countryEgdes.Select(x =>
            {
                var _tile = ScriptableObject.CreateInstance<Tile>();
                _tile.sprite = x;
                return _tile;
            }).ToArray();
        }

        return _countryEdgeTiles[index];
    }
}
