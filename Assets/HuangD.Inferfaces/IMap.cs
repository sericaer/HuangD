using Math.TileMap;
using System.Collections.Generic;

namespace HuangD.Interfaces
{
    public interface IMap
    {
        public Dictionary<(int x, int y), TerrainType> terrains { get; set; }

        public Dictionary<Block, TerrainType> blocks { get; set; }
    }

    public interface ICountry
    {
        public string name { get; }
        IEnumerable<IProvince> provinces { get; }
        public (float r, float g, float b) color { get; }
    }
}


