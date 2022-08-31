using HuangD.Interfaces;
using Math.TileMap;
using Maths;
using System;
using System.Collections.Generic;
using System.Linq;

namespace HuangD.Maps
{
    static class TerrainBuilder
    {
        public static GRandom random { get; set; }

        public static Dictionary<(int x, int y), TerrainType>  Build(IEnumerable<Block> blocks, int mapSize)
        {
            var rslt = new Dictionary<(int x, int y), TerrainType>();

            var terrainBlocks = GroupByTerrainType(blocks, mapSize);

            foreach (var dict in terrainBlocks.Select(x => GenrateTerrain(x.Key, x.Value)))
            {
                foreach (var pair in dict)
                {
                    rslt.Add(pair.Key, pair.Value);
                }
            }

            return rslt;
        }

        private static Dictionary<(int x, int y), TerrainType> GenrateTerrain(TerrainType terrainType, IEnumerable<Block> blocks)
        {
            switch (terrainType)
            {
                case TerrainType.Mount:
                    return GenrateMount(blocks);
                case TerrainType.Hill:
                    return GenrateHill(blocks);
                case TerrainType.Plain:
                    return GenratePlain(blocks);
                case TerrainType.Water:
                    return GenrateWater(blocks);
                default:
                    throw new System.Exception();
            }
        }

        private static Dictionary<(int x, int y), TerrainType> GenrateWater(IEnumerable<Block> blocks)
        {
            var rslt = new Dictionary<(int x, int y), TerrainType>();

            foreach (var elem in blocks.SelectMany(x => x.elements))
            {
                rslt.Add(elem, TerrainType.Water);
            }

            return rslt;
        }

        private static Dictionary<(int x, int y), TerrainType> GenratePlain(IEnumerable<Block> blocks)
        {
            var rslt = new Dictionary<(int x, int y), TerrainType>();

            var elements = blocks.SelectMany(x => x.elements).OrderBy(_ => random.getNum(0, int.MaxValue)).ToList();
            var edges = blocks.SelectMany(x => x.edges).ToHashSet();

            while (elements.Count > rslt.Count * 10)
            {
                var seeds = elements.Take(30).ToArray();
                foreach (var seed in seeds)
                {
                    rslt.Add(seed, TerrainType.Hill);
                    elements.Remove(seed);
                }

                foreach (var selected in rslt.Keys.ToArray())
                {
                    var neighbors = Hexagon.GetNeighbors(selected).Where(x => !edges.Contains(x));
                    var vailds = neighbors.Intersect(elements).ToArray();
                    if (vailds.Length == 0)
                    {
                        continue;
                    }

                    var index = random.getNum(0, vailds.Length);
                    rslt.Add(vailds[index], TerrainType.Hill);
                    elements.Remove(vailds[index]);
                }
            }

            foreach (var elem in elements)
            {
                rslt.Add(elem, TerrainType.Plain);
            }

            return rslt;
        }

        private static Dictionary<(int x, int y), TerrainType> GenrateHill(IEnumerable<Block> blocks)
        {
            var rslt = new Dictionary<(int x, int y), TerrainType>();

            var elements = blocks.SelectMany(x => x.elements).OrderBy(_ => random.getNum(0, int.MaxValue)).ToList();
            var edges = blocks.SelectMany(x => x.edges).ToHashSet();

            while (elements.Count > rslt.Count * 10)
            {
                var seeds = elements.Take(100).ToArray();
                foreach (var seed in seeds)
                {
                    rslt.Add(seed, TerrainType.Plain);
                    elements.Remove(seed);
                }

                foreach (var selected in rslt.Keys.ToArray())
                {
                    var neighbors = Hexagon.GetNeighbors(selected).Where(x => !edges.Contains(x));
                    var vailds = neighbors.Intersect(elements).ToArray();
                    if (vailds.Length == 0)
                    {
                        continue;
                    }

                    var index = random.getNum(0, vailds.Length);
                    rslt.Add(vailds[index], TerrainType.Plain);
                    elements.Remove(vailds[index]);
                }
            }

            var mounts = new List<(int x, int y)>();
            while (elements.Count > mounts.Count * 10)
            {
                var seeds = elements.Take(10).ToArray();
                foreach (var seed in seeds)
                {
                    mounts.Add(seed);
                    elements.Remove(seed);
                }

                foreach (var selected in mounts.ToArray())
                {
                    var neighbors = Hexagon.GetNeighbors(selected);
                    var vailds = neighbors.Intersect(elements).ToArray();
                    if (vailds.Length == 0)
                    {
                        continue;
                    }

                    var index = random.getNum(0, vailds.Length);
                    mounts.Add(vailds[index]);
                    elements.Remove(vailds[index]);
                }
            }

            foreach (var mount in mounts)
            {
                rslt.Add(mount, TerrainType.Mount);
            }

            foreach (var elem in elements)
            {
                rslt.Add(elem, TerrainType.Hill);
            }

            return rslt;
        }

        private static Dictionary<(int x, int y), TerrainType> GenrateMount(IEnumerable<Block> blocks)
        {
            var elements = blocks.SelectMany(x => x.elements).OrderBy(_ => random.getNum(0, int.MaxValue)).ToList();
            var edges = blocks.SelectMany(x => x.edges).ToHashSet();

            var rslt = new Dictionary<(int x, int y), TerrainType>();
            while (elements.Count > rslt.Count * 3)
            {
                var seeds = elements.Take(20).ToArray();
                foreach (var seed in seeds)
                {
                    rslt.Add(seed, TerrainType.Hill);
                    elements.Remove(seed);
                }

                foreach (var selected in rslt.Keys.ToArray())
                {
                    var neighbors = Hexagon.GetNeighbors(selected).Where(x => !edges.Contains(x));
                    var vailds = neighbors.Intersect(elements).ToArray();
                    if (vailds.Length == 0)
                    {
                        continue;
                    }

                    var index = random.getNum(0, vailds.Length);
                    rslt.Add(vailds[index], TerrainType.Hill);
                    elements.Remove(vailds[index]);
                }
            }

            var plains = new List<(int x, int y)>();
            while (rslt.Count > plains.Count * 4)
            {
                var seeds = rslt.Keys.Take(20).ToArray();
                foreach (var seed in seeds)
                {
                    plains.Add(seed);
                }

                foreach (var selected in plains.ToArray())
                {
                    var neighbors = Hexagon.GetNeighbors(selected);
                    var vailds = neighbors.Intersect(rslt.Keys).ToArray();
                    if (vailds.Length == 0)
                    {
                        continue;
                    }

                    var index = random.getNum(0, vailds.Length);
                    plains.Add(vailds[index]);
                }
            }

            foreach (var plain in plains)
            {
                rslt[plain] = TerrainType.Plain;
            }

            foreach (var elem in elements)
            {
                rslt.Add(elem, TerrainType.Mount);
            }

            return rslt;
        }

        internal static Dictionary<TerrainType, IEnumerable<Block>> GroupByTerrainType(IEnumerable<Block> blocks, int mapSize)
        {
            var rslt = new Dictionary<TerrainType, IEnumerable<Block>>();

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

            rslt.Add(TerrainType.Hill, hills);
            rslt.Add(TerrainType.Plain, plains);
            rslt.Add(TerrainType.Water, waters);
            rslt.Add(TerrainType.Mount, mounts.Union(mountPlus));

            return rslt;
        }
    }
}
