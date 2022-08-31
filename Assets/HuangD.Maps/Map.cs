using HuangD.Interfaces;
using Math.TileMap;
using System.Collections.Generic;

namespace HuangD.Maps
{
    public partial class Map : IMap
    {
        public Dictionary<(int x, int y), TerrainType> terrains { get; set; }

        public IEnumerable<IProvince> provinces { get; set; }
        public IEnumerable<Block> blocks { get; set; }
        public Dictionary<IProvince, Block> province2Block { get; set; }
    }
}
