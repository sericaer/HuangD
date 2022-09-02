using Math.TileMap;
using System.Collections.Generic;

namespace HuangD.Interfaces
{
    public interface IMap
    {
        public Dictionary<(int x, int y), TerrainType> terrains { get; set; }

        public Dictionary<Block, TerrainType> blocks { get; set; }
    }
}


