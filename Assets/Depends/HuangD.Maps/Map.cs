using HuangD.Interfaces;
using Math.TileMap;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace HuangD.Maps
{
    public partial class Map : IMap
    {
        public Dictionary<(int x, int y), float> nosieMap { get;  set; }

        public Dictionary<(int x, int y), float> heightMap { get; set; }
        public Dictionary<(int x, int y), TerrainType> terrains { get; set; }

        public Dictionary<(int x, int y), float> rainMap { get; set; }

        public Dictionary<(int x, int y), float> wetnessMap { get; set; }

        public Dictionary<Block, TerrainType> blocks { get; set; }
        public Dictionary<(int x, int y), int> rivers { get; set; }

        public Dictionary<(int x, int y), BiomeType> biomesMap { get; set; }

        public Dictionary<(int x, int y), int> populationMap { get; set; }

        public IBlockMap blockMap { get; set; }

        public IRiverMap riverMap { get; set; }
    }

    class Cell : ICell
    {
        public (int x, int y) position { get; set; }
        public float noise { get; set; }
        public int block { get; set; }
        public bool isBlockEdge { get; set; }
        public float height { get; set; }
        public TerrainType terrain { get; set; }
        public float rain { get; set; }
        public float wetness { get; set; }
        public ILandInfo landInfo { get; set; }
    }

    class BlockMap : IBlockMap
    {
        public ICell this[(int x, int y) key] => dict[key];

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
