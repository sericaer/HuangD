using HuangD.Interfaces;
using Math.TileMap;
using System.Collections.Generic;

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
    }
}
