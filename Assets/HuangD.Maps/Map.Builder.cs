using HuangD.Entities;
using HuangD.Interfaces;
using Math.TileMap;
using System.Collections.Generic;
using System.Linq;

namespace HuangD.Maps
{
    public partial class Map
    {
        public static class Builder
        {
            public static IMap Build(int mapSize, string seed)
            {
                var map = new Map();

                var blocksBuilder = new Block.BuilderGroup(mapSize, seed);
                map.blocks = blocksBuilder.Build();

                TerrainBuilder.random = new Maths.GRandom(seed);
                map.terrains = TerrainBuilder.Build(map.blocks, mapSize);

                map.provinces = Province.Builder.build(map.blocks.Count(), seed);

                map.province2Block = new Dictionary<IProvince, Block>();
                for (int i=0; i<map.blocks.Count(); i++)
                {
                    map.province2Block.Add(map.provinces.ElementAt(i), map.blocks.ElementAt(i));
                }

                return map;
            }
        }
    }
}
