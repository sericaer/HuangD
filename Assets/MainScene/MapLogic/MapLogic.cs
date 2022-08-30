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
    }

    public void ScrollWheel(bool flag)
    {
        mapGrid.transform.localScale = CalcNextScale(flag);
    }

    internal void SetBlocks(Dictionary<Block, Color> block2Color)
    {
        foreach(var pair in block2Color)
        {
            foreach(var pos in pair.Key.elements)
            {
                blockMap.SetCell(new Vector3Int(pos.x, pos.y), pair.Value);
            }
        }

        var edges = GenerateEdges(block2Color.Keys.ToArray());
        foreach(var pair in edges)
        {
            edgeMap.SetCell(new Vector3Int(pair.Key.x, pair.Key.y), pair.Value);
        }
    }

    internal void SetTerrain(Dictionary<(int x, int y), TerrainType> pos2Terrain)
    {
        foreach (var pair in pos2Terrain)
        {
            terrainMap.SetCell(new Vector3Int(pair.Key.x, pair.Key.y), pair.Value);
        }
    }

    private Dictionary<(int x, int y), int> GenerateEdges(Block[] blocks)
    {
        var dict = new Dictionary<(int x, int y), Block>();
        foreach (var block in blocks)
        {
            foreach (var edge in block.edges.Select(e => Hexagon.ScaleOffset(e, 2)))
            {
                if (dict.ContainsKey(edge))
                {
                    continue;
                }
                dict.Add(edge, block);
            }
        }

        var rlst = new Dictionary<(int x, int y), int>();
        var edgeCenters = blocks.SelectMany(x => x.edges)
            .Select(x => Hexagon.ScaleOffset(x, 2))
            .ToHashSet();

        var mirrorDrects = new (int d1, int d2)[]
        {
            (0, 3),
            (2, 5),
            (1, 4),
        };

        foreach (var egde in edgeCenters.SelectMany(e => Hexagon.GetNeighbors(e)).Distinct())
        {
            for (int i = 0; i < mirrorDrects.Length; i++)
            {
                var mirrorDiect = mirrorDrects[i];

                var neighbor1 = Hexagon.GetNeighbor(egde, mirrorDiect.d1);
                var neighbor2 = Hexagon.GetNeighbor(egde, mirrorDiect.d2);

                if (!dict.ContainsKey(neighbor1) || !dict.ContainsKey(neighbor2))
                {
                    continue;
                }

                if (dict[neighbor1] != dict[neighbor2])
                {
                    rlst.Add(egde, i);
                    break;
                }
            }
        }

        return rlst;
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
