using HuangD.Interfaces;
using Math.TileMap;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace HuangD.Maps
{
    public partial class Map : IMap
    {
        public IBlockMap blockMap { get; set; }

        public IRiverMap riverMap { get; set; }

    }

    class Cell : ICell
    {
        public static Func<ICell, IProvince> funcGetProvince;
        public static Func<ICell, IEnumerable<ICell>> funcGetNeighbors;

        public (int x, int y) position { get; set; }
        public float noise { get; set; }
        public int block { get; set; }
        public bool isBlockEdge { get; set; }
        public float height { get; set; }
        public TerrainType terrain { get; set; }
        public float rain { get; set; }
        public float wetness { get; set; }
        public ILandInfo landInfo { get; set; }
        public IProvince province => funcGetProvince(this);

        public IEnumerable<ICell> neighors => funcGetNeighbors(this);
    }

    class BlockMap : IBlockMap
    {
        public ICell this[(int x, int y) key]
        {
            get
            {
                ICell cell;
                if(dict.TryGetValue(key, out cell))
                {
                    return cell;
                }
                else
                {
                    return null;
                }
            }
        }
        public BlockMap(IEnumerable<ICell> cells)
        {
            dict = cells.ToDictionary(k=>k.position, v=>v);
        }

        public IEnumerator<ICell> GetEnumerator()
        {
            return dict.Values.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        private Dictionary<(int x, int y), ICell> dict;
    }

    class RiverMap : IRiverMap
    {
        public class Item : IRiverItem
        {
            public (int x, int y) position { get; set; }
            public int index { get; set; }
        }

        public RiverMap(IEnumerable<IRiverItem> items)
        {
            this.items = items.ToArray();
        }

        public IEnumerator<IRiverItem> GetEnumerator()
        {
            foreach (var item in items)
            {
                yield return item;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {

            return this.GetEnumerator();
        }

        private IRiverItem[] items;
    }

    class LandInfo : ILandInfo
    {
        public BiomeType biome { get; set; }
        public int population { get; set; }
    }
}
