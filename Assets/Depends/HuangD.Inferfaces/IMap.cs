using Math.TileMap;
using System.Collections.Generic;

namespace HuangD.Interfaces
{
    public interface IMap 
    {
        public IBlockMap blockMap { get;  }
        public IRiverMap riverMap { get; }
    }

    public interface IBlockMap : IEnumerable<ICell>
    {
        public ICell this[(int x, int y) key] { get; }

    }

    public interface IRiverMap : IEnumerable<IRiverItem>
    {

    }

    public interface IRiverItem
    {
        public (int x, int y) position { get; set; }
        int index { get; set; }
    }

    public interface ICell
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

        public int? provinceId { get; set; }
    }

    public interface ILandInfo
    {
        public BiomeType biome { get; set; }
        public int population { get; set; }
    }

    public enum BiomeType
    {
        Desert_Plain,
        Desert_Hill,
        Desert_Mount,

        Grass_Plain,
        Grass_Hill,
        Grass_Mount,

        Farm_Plain,
        Farm_Hill,

        Forest_Plain,
        Forest_Hill,

        Juggle_Plain,
        Juggle_Hill,

        Marsh_Plain
    }
}


