using HuangD.Entities;
using HuangD.Interfaces;
using Math.TileMap;
using Maths;
using System.Collections.Generic;
using System.Linq;

namespace HuangD.Maps
{
    public class MapInit
    {
        public int width;
        public int high;
    }

    public partial class Map
    {
        public static class Builder
        {
            public static IMap Build(MapInit mapInit, string seed, System.Action<string> processInfo)
            {

                var random = new GRandom(seed);

                processInfo.Invoke("创建噪音图");
                var noiseMap = NoiseMapBuilder.Build(random, mapInit.high, mapInit.width);

                processInfo.Invoke("划分地块");
                var blocks = BlockBuilder.Build(noiseMap, random).ToArray();

                processInfo.Invoke("创建地形");
                var block2Terrain = GroupByTerrainType(blocks, mapInit, random);

                var terrains = TerrainBuilder.Build(block2Terrain, noiseMap, random);

                //processInfo.Invoke("创建河流");
                //var rivers = RiverBuilder.Build(blocks, terrains, random);

                var map = new Map();
                map.blocks = block2Terrain;
                map.terrains = terrains;
                map.rivers = new Dictionary<(int x, int y), int>();

                return map;
            }

            internal static Dictionary<Block, TerrainType> GroupByTerrainType(Block[] blocks, MapInit mapInit, GRandom random)
            {

                var dict = new Dictionary<Block, TerrainType>();
                foreach(var block in blocks.Where(x => x.edges.Any(r => r.y >= mapInit.width * 0.7 || r.x == 0)))
                {
                    dict.Add(block, TerrainType.Water);
                }

                blocks = blocks.Except(dict.Keys).ToArray();

                foreach (var block in blocks.Where(x => x.edges.Any(r => r.y == 0)))
                {
                    dict.Add(block, TerrainType.Mount);
                }

                var queue = new Queue<Block>(blocks.Except(dict.Keys));
                while(queue.Count != 0)
                {
                    var block = queue.Dequeue();

                    var neighors = dict.Keys.Where(b => b.isNeighbor(block)).ToArray();
                    if(neighors.Length == 0)
                    {
                        queue.Enqueue(block);
                        continue;
                    }

                    var terrainType = CalcTerrainType(neighors.Select(n=> dict[n]), random);
                    dict.Add(block, terrainType);
                }

                return dict;

                //var waters = blocks.Where(x => x.edges.Any(r => r.y >= mapInit.width - 2 || r.x == 0)).ToList();

                //blocks = blocks.Except(waters).ToArray();

                //var mounts = blocks.Where(x => x.edges.Any(r => r.y == 0)).ToList();

                //blocks = blocks.Except(mounts).ToArray();

                //var hills = new List<Block>();
                //var plains = new List<Block>();

                //var queue = new Queue<Block>(blocks);

                //while(queue.Count != 0)
                //{

                //}

                //foreach(var block in blocks)
                //{
                //    var percentGroup = new Dictionary<TerrainType, int>()
                //    {
                //        {TerrainType.Mount, 0 },
                //        {TerrainType.Hill, 0 },
                //        {TerrainType.Plain, 0 },
                //        {TerrainType.Water, 0 },
                //    };

                //    if (waters.Any(m => block.isNeighbor(m)))
                //    {
                //        percentGroup[TerrainType.Water] += 50;
                //        percentGroup[TerrainType.Plain] += 10;
                //    }
                //    if (plains.Any(m => block.isNeighbor(m)))
                //    {
                //        percentGroup[TerrainType.Hill] += 10;
                //        percentGroup[TerrainType.Plain] +=10;
                //    }
                //    if (hills.Any(m => block.isNeighbor(m)))
                //    {
                //        percentGroup[TerrainType.Hill] += 20;
                //        percentGroup[TerrainType.Plain] += 20;
                //        percentGroup[TerrainType.Mount] += 10;
                //    }
                //    if (mounts.Any(m => block.isNeighbor(m)))
                //    {
                //        percentGroup[TerrainType.Hill] += 10;
                //        percentGroup[TerrainType.Mount] += 20;
                //    }

                //    var terrainType = random.RandomInGroup<TerrainType>(percentGroup.Select(x => (x.Value, x.Key)));
                //    switch(terrainType)
                //    {
                //        case TerrainType.Hill:
                //            hills.Add(block);
                //            break;
                //        case TerrainType.Plain:
                //            plains.Add(block);
                //            break;
                //        case TerrainType.Water:
                //            waters.Add(block);
                //            break;
                //        case TerrainType.Mount:
                //            mounts.Add(block);
                //            break;
                //    }
                //}

                //var rslt = new Dictionary<Block, TerrainType>();
                //foreach(var hill in hills)
                //{
                //    rslt.Add(hill, TerrainType.Hill);
                //}

                //foreach (var plain in plains)
                //{
                //    rslt.Add(plain, TerrainType.Plain);
                //}

                //foreach (var mount in mounts)
                //{
                //    rslt.Add(mount, TerrainType.Mount);
                //}

                //foreach (var water in waters)
                //{
                //    rslt.Add(water, TerrainType.Water);
                //}

                //return rslt;
            }

            private static TerrainType CalcTerrainType(IEnumerable<TerrainType> neighors, GRandom random)
            {
                var percentGroup = new Dictionary<TerrainType, int>()
                    {
                        {TerrainType.Mount, 0 },
                        {TerrainType.Hill, 0 },
                        {TerrainType.Plain, 0 },
                        {TerrainType.Water, 0 },
                    };

                foreach (var type in neighors.OrderBy(_=>random.getNum(0, int.MaxValue)))
                {

                    switch(type)
                    {
                        case TerrainType.Water:
                            percentGroup[TerrainType.Plain] += 10;
                            break;
                        case TerrainType.Plain:
                            percentGroup[TerrainType.Hill] += 10;
                            percentGroup[TerrainType.Plain] += 10;
                            break;
                        case TerrainType.Hill:
                            percentGroup[TerrainType.Hill] += 20;
                            percentGroup[TerrainType.Plain] += 20;
                            percentGroup[TerrainType.Mount] += 10;
                            break;
                        case TerrainType.Mount:
                            percentGroup[TerrainType.Hill] += 10;
                            percentGroup[TerrainType.Mount] += 20;
                            break;
                        default:
                            throw new System.Exception($"not support type:{type}");
                    }
                }

                return random.RandomInGroup<TerrainType>(percentGroup.Select(x => (x.Value, x.Key)));
            }
        }
    }
}
