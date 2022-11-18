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

        public IEnumerable<ICell> neighors { get; }
        public IProvince province { get; }
    }

    public interface ILandInfo
    {
        public BiomeType biome { get; set; }
        public int population { get; set; }
    }

    public enum BiomeType
    {
        沙漠,
        戈壁,
        荒山,

        草原,
        山丘草原,
        高山草原,

        农田,
        梯田,

        林地,
        山丘林地,

        雨林,

        沼泽
    }
}


