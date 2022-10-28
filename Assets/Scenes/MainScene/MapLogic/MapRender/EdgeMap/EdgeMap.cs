using HuangD.Interfaces;
using Math.TileMap;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

public class EdgeMap : MapBehaviour
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

    class CellPair
    {
        public ICell c1;
        public ICell c2;

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            var peer = obj as CellPair;
            if(peer == null)
            {
                return false;
            }

            return (this.c1 == peer.c1 && this.c2 == peer.c2) || (this.c2 == peer.c1 && this.c1 == peer.c2);
        }

    }

    private HashSet<CellPair> provinceEdgeCellPairs;

    internal void SetCountries(IEnumerable<ICountry> countries)
    {
        foreach (var cellpair in provinceEdgeCellPairs.Where(x => x.c1.province.country != x.c2.province.country))
        {
            var scaleCell = Hexagon.GetIntersectScale(cellpair.c1.position, cellpair.c2.position);
            tilemap.SetTileColor(new Vector3Int(scaleCell.x, scaleCell.y), tile, Color.black);
        }
    }

    internal void SetProvinces(IEnumerable<IProvince> provinces)
    {
        Debug.Log("edgeMap SetProvinces Start");

        provinceEdgeCellPairs = new HashSet<CellPair>();

        var checkedCells = new HashSet<ICell>();

        foreach(var cell in provinces.SelectMany(x=>x.cells))
        {
            checkedCells.Add(cell);

            foreach (var neighbor in cell.neighors.Where(n=> !checkedCells.Contains(n)))
            {
                if(neighbor.province != null && neighbor.province != cell.province)
                {
                    provinceEdgeCellPairs.Add(new CellPair() { c1 = cell, c2 = neighbor });
                }
            }
        }

        foreach (var cellpair in provinceEdgeCellPairs)
        {
            var scaleCell = Hexagon.GetIntersectScale(cellpair.c1.position, cellpair.c2.position);
            tilemap.SetTileColor(new Vector3Int(scaleCell.x, scaleCell.y), tile, Color.gray);
        }

        Debug.Log("edgeMap SetProvinces end");
    }
}
