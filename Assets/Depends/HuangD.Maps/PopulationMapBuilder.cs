using HuangD.Interfaces;
using Maths;
using System;
using System.Collections.Generic;
using System.Linq;

namespace HuangD.Maps
{
    internal class PopulationMapBuilder
    {
        internal static Dictionary<(int x, int y), int> Build(Dictionary<(int x, int y), BiomeType> biomesMap, GRandom random)
        {
            return biomesMap.ToDictionary(k=>k.Key, v=>
            {
                switch(v.Value)
                {
                    case BiomeType.Desert_Plain:
                    case BiomeType.Desert_Hill:
                    case BiomeType.Desert_Mount:
                        return random.getNum(0,100);

                    case BiomeType.Grass_Mount:
                        return random.getNum(100, 200);

                    case BiomeType.Grass_Hill:
                        return random.getNum(200, 500);

                    case BiomeType.Grass_Plain:
                        return random.getNum(500, 1000);

                    case BiomeType.Farm_Hill:
                        return random.getNum(3000, 5000);

                    case BiomeType.Farm_Plain:
                        return random.getNum(6000, 10000);

                    case BiomeType.Forest_Plain:
                        return random.getNum(500, 1000);

                    case BiomeType.Forest_Hill:
                        return random.getNum(300, 500);

                    case BiomeType.Juggle_Plain:
                    case BiomeType.Juggle_Hill:
                        return random.getNum(100, 200);

                    case BiomeType.Marsh_Plain:
                        return random.getNum(0, 100);

                    default:
                        throw new Exception();
                }
            });
        }
    }
}