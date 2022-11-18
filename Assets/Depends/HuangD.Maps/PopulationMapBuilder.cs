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
            return biomesMap.ToDictionary(k => k.Key, (Func<KeyValuePair<(int x, int y), BiomeType>, int>)(v=>
            {
                switch(v.Value)
                {
                    case BiomeType.沙漠:
                    case BiomeType.戈壁:
                    case BiomeType.荒山:
                        return random.getNum(0,100);

                    case BiomeType.高山草原:
                        return random.getNum(100, 200);

                    case BiomeType.山丘草原:
                        return random.getNum(200, 500);

                    case BiomeType.草原:
                        return random.getNum(500, 1000);

                    case BiomeType.梯田:
                        return random.getNum(3000, 5000);

                    case BiomeType.农田:
                        return random.getNum(6000, 10000);

                    case BiomeType.林地:
                        return random.getNum(500, 1000);

                    case BiomeType.山丘林地:
                        return random.getNum(300, 500);

                    case BiomeType.雨林:
                        return random.getNum(100, 200);

                    case BiomeType.沼泽:
                        return random.getNum(0, 100);

                    default:
                        throw new Exception();
                }
            }));
        }
    }
}