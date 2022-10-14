using Math.TileMap;
using System.Collections.Generic;

namespace HuangD.Interfaces
{
    public interface IMap
    {
        public Dictionary<(int x, int y), float> nosieMap { get; set; }
        public Dictionary<(int x, int y), float> heightMap { get; set; }
        public Dictionary<(int x, int y), float> rainMap { get; set; }
        public Dictionary<(int x, int y), float> wetnessMap { get; set; }
        public Dictionary<(int x, int y), TerrainType> terrains { get; set; }
        public Dictionary<(int x, int y), int> rivers { get; set; }
        public Dictionary<Block, TerrainType> blocks { get; set; }
        public Dictionary<(int x, int y), BiomeType> biomesMap { get; set; }

        public Dictionary<(int x, int y), int> populationMap { get; set; }
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


