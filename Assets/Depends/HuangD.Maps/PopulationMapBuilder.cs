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
                        return 100;

                    case BiomeType.Grass_Plain:
                        return 1000;

                    case BiomeType.Grass_Hill:
                        return 500;

                    case BiomeType.Grass_Mount:
                        return 200;

                    case BiomeType.Farm_Plain:
                        return 10000;

                    case BiomeType.Farm_Hill:
                        return 3000;

                    case BiomeType.Forest_Plain:
                        return 800;

                    case BiomeType.Forest_Hill:
                        return 300;

                    case BiomeType.Juggle_Plain:
                    case BiomeType.Juggle_Hill:
                        return 150;

                    case BiomeType.Marsh_Plain:
                        return 100;

                    default:
                        throw new Exception();
                }
            });
        }
    }
}