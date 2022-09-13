using HuangD.Entities;
using HuangD.Interfaces;
using Math.TileMap;
using Maths;
using System.Collections.Generic;
using System.Linq;

namespace HuangD.Maps
{

    public partial class Map
    {
        public static class Builder
        {
            public static IMap Build(int mapSize, string seed, System.Action<string> processInfo)
            {

                var random = new GRandom(seed);

                processInfo.Invoke("创建区块");

                var blocksBuilder = new Block.BuilderGroup(mapSize, random);
                var blocks = blocksBuilder.Build();

                processInfo.Invoke("创建地形");
                var block2Terrain = GroupByTerrainType(blocks, mapSize, random);
                var terrains = TerrainBuilder.Build(block2Terrain, random);

                processInfo.Invoke("创建河流");
                var rivers = RiverBuilder.Build(block2Terrain, terrains, random);

                var map = new Map();
                map.blocks = block2Terrain;
                map.terrains = terrains;
                map.rivers = rivers;

                return map;
            }

            internal static Dictionary<Block, TerrainType> GroupByTerrainType(IEnumerable<Block> blocks, int mapSize, GRandom random)
            {
                var waters = blocks.Where(x => x.edges.Any(r => r.y == mapSize - 1 || r.x == 0)).ToList();

                blocks = blocks.Except(waters).ToArray();

                var mounts = blocks.Where(x => x.edges.Any(r => r.y == 0)).ToList();

                blocks = blocks.Except(mounts).ToArray();

                var hills = new List<Block>();
                var plains = new List<Block>();

                foreach(var block in blocks)
                {
                    var percentGroup = new Dictionary<TerrainType, int>()
                    {
                        {TerrainType.Mount, 0 },
                        {TerrainType.Hill, 5 },
                        {TerrainType.Plain, 5 },
                        {TerrainType.Water, 0 },
                    };

                    if (waters.Any(m => block.isNeighbor(m)))
                    {
                        percentGroup[TerrainType.Water] += 20;
                        percentGroup[TerrainType.Plain] += 10;
                    }
                    if (plains.Any(m => block.isNeighbor(m)))
                    {
                        percentGroup[TerrainType.Hill] += 10;
                        percentGroup[TerrainType.Plain] +=10;
                    }
                    if (hills.Any(m => block.isNeighbor(m)))
                    {
                        percentGroup[TerrainType.Hill] += 20;
                        percentGroup[TerrainType.Plain] += 20;
                        percentGroup[TerrainType.Mount] += 10;
                    }
                    if (mounts.Any(m => block.isNeighbor(m)))
                    {
                        percentGroup[TerrainType.Hill] += 10;
                        percentGroup[TerrainType.Mount] += 20;
                    }

                    var terrainType = random.RandomInGroup<TerrainType>(percentGroup.Select(x => (x.Value, x.Key)));
                    switch(terrainType)
                    {
                        case TerrainType.Hill:
                            hills.Add(block);
                            break;
                        case TerrainType.Plain:
                            plains.Add(block);
                            break;
                        case TerrainType.Water:
                            waters.Add(block);
                            break;
                        case TerrainType.Mount:
                            mounts.Add(block);
                            break;
                    }
                }

                var rslt = new Dictionary<Block, TerrainType>();
                foreach(var hill in hills)
                {
                    rslt.Add(hill, TerrainType.Hill);
                }

                foreach (var plain in plains)
                {
                    rslt.Add(plain, TerrainType.Plain);
                }

                foreach (var mount in mounts)
                {
                    rslt.Add(mount, TerrainType.Mount);
                }

                foreach (var water in waters)
                {
                    rslt.Add(water, TerrainType.Water);
                }

                return rslt;
            }
        }
    }
}
