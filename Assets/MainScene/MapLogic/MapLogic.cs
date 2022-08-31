using HuangD.Interfaces;
using HuangD.Maps;
using Math.TileMap;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MapLogic : MonoBehaviour
{
    public Camera mapCamera;
    public Grid mapGrid;

    public BlockMap blockMap;
    public EdgeMap edgeMap;
    public TerrainMap terrainMap;

    public ProvinceNames provinceNames;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnMove(Vector3 pos)
    {
        Vector3 move = CaclMoveOffset(pos);

        mapCamera.transform.position = mapCamera.transform.position + move;
        provinceNames.UpdateNamePosition();
    }

    public void ScrollWheel(bool flag)
    {
        mapGrid.transform.localScale = CalcNextScale(flag);
        provinceNames.UpdateNamePosition();
    }

    internal void SetMapData(IMap map)
    {
        foreach (var pair in map.terrains)
        {
            terrainMap.SetCell(new Vector3Int(pair.Key.x, pair.Key.y), pair.Value);
        }

        foreach (var pair in map.province2Block)
        {
            var color = new Color(pair.Key.color.r, pair.Key.color.g, pair.Key.color.b);
            foreach (var pos in pair.Value.elements)
            {
                blockMap.SetCell(new Vector3Int(pos.x, pos.y), color);
            }
        }

        provinceNames.SetProvinces(map.province2Block);

        var edges = Utilty.GenerateEdges(map.blocks);
        foreach (var pair in edges)
        {
            edgeMap.SetCell(new Vector3Int(pair.Key.x, pair.Key.y), pair.Value);
        }
    }

    private Vector3 CaclMoveOffset(Vector3 pos)
    {
        Vector3 move = (pos - new Vector3(0.5f, 0.5f)) * 0.1f;
        Debug.Log(move);
        if (move.x < 0)
        {
            var leftEdgeCenter = mapCamera.ViewportToWorldPoint(new Vector3(0f, 0.5f));
            var cellIndex = blockMap.tilemap.WorldToCell(leftEdgeCenter);
            if (!blockMap.tilemap.HasTile(new Vector3Int(cellIndex.x, cellIndex.y)))
            {
                move = new Vector3(0, move.y);
            }
        }
        else if (move.x > 0)
        {
            var leftEdgeCenter = mapCamera.ViewportToWorldPoint(new Vector3(1f, 0.5f));
            var cellIndex = blockMap.tilemap.WorldToCell(leftEdgeCenter);
            if (!blockMap.tilemap.HasTile(new Vector3Int(cellIndex.x, cellIndex.y)))
            {
                move = new Vector3(0, move.y);
            }
        }

        if (move.y < 0)
        {
            var leftEdgeCenter = mapCamera.ViewportToWorldPoint(new Vector3(0.5f, 0f));
            var cellIndex =blockMap.tilemap.WorldToCell(leftEdgeCenter);
            if (!blockMap.tilemap.HasTile(new Vector3Int(cellIndex.x, cellIndex.y)))
            {
                move = new Vector3(move.x, 0);
            }
        }
        else if (move.y > 0)
        {
            var leftEdgeCenter = mapCamera.ViewportToWorldPoint(new Vector3(0.5f, 1f));
            var cellIndex = blockMap.tilemap.WorldToCell(leftEdgeCenter);
            if (!blockMap.tilemap.HasTile(new Vector3Int(cellIndex.x, cellIndex.y)))
            {
                move = new Vector3(move.x, 0);
            }
        }

        return move;
    }

    private Vector3 CalcNextScale(bool flag)
    {
        var newScale = mapGrid.transform.localScale * (flag ? 1.1f : 0.9f);
        if (newScale.x < 0.3f || newScale.x > 1.0f)
        {
            return mapGrid.transform.localScale;
        }

        var center = mapCamera.ViewportToWorldPoint(new Vector3(0.5f, 0.5f));
        var cellIndex = blockMap.tilemap.WorldToCell(center);
        if (!blockMap.tilemap.HasTile(new Vector3Int(cellIndex.x, cellIndex.y)))
        {
            return mapGrid.transform.localScale;
        }

        return newScale;
    }
}
