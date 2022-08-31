using Math.TileMap;
using System.Collections.Generic;

namespace HuangD.Interfaces
{
    public interface IMap
    {
        public Dictionary<(int x, int y), TerrainType> terrains { get; set; }

        public IEnumerable<Block> blocks { get; set; }
        public IEnumerable<IProvince> provinces { get; set; }

        public Dictionary<IProvince, Block> province2Block { get; set; }
    }
}


