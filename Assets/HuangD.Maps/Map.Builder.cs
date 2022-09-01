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
            public static IMap Build(int mapSize, string seed)
            {

                var random = new GRandom(seed);

                var blocksBuilder = new Block.BuilderGroup(mapSize, random);
                var blocks = blocksBuilder.Build();

                var block2Terrain = GroupByTerrainType(blocks, mapSize, random);
                var terrains = TerrainBuilder.Build(block2Terrain, random);

                var map = new Map();
                map.blocks = block2Terrain;
                map.terrains = terrains;

                return map;
            }

            internal static Dictionary<Block, TerrainType> GroupByTerrainType(IEnumerable<Block> blocks, int mapSize, GRandom random)
            {
                var waters = blocks.Where(x => x.edges.Any(r => r.y == mapSize - 1 || r.x == 0));

                blocks = blocks.Except(waters).ToArray();

                var mounts = blocks.Where(x => x.edges.Any(r => r.y == 0));

                blocks = blocks.Except(mounts).ToArray();

                var hills = new List<Block>();
                var plains = new List<Block>();
                var mountPlus = new List<Block>();

                foreach (var block in blocks)
                {
                    if (mounts.Any(m => block.isNeighbor(m)))
                    {
                        if (random.getNum(0, 3) < 1)
                        {
                            hills.Add(block);
                        }
                        else
                        {
                            mountPlus.Add(block);
                        }
                    }

                    else if (waters.Any(m => block.isNeighbor(m)))
                    {
                        plains.Add(block);
                    }
                    else
                    {
                        var hillPercent = 50;
                        var plainPercent = 50;

                        if (hills.Any(m => block.isNeighbor(m)))
                        {
                            hillPercent += 50;
                        }
                        if (hills.Any(m => block.isNeighbor(m)))
                        {
                            plainPercent += 100;
                        }

                        var randomValue = random.getNum(0, hillPercent + plainPercent);
                        if (randomValue < hillPercent)
                        {
                            hills.Add(block);
                        }
                        else
                        {
                            plains.Add(block);
                        }
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

                foreach (var mount in mounts.Union(mountPlus))
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
