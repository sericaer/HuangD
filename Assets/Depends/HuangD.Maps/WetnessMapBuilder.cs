using Math.TileMap;
using HuangD.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace HuangD.Maps
{
    internal class WetnessMapBuilder
    {
        internal static Dictionary<(int x, int y), float> Build(Dictionary<(int x, int y), HashSet<(int x, int y)>> riverBanks, Dictionary<(int x, int y), float> rainMap, Dictionary<(int x, int y), TerrainType> terrains)
        {
            var plusMap = new Dictionary<(int x, int y), float>();

            foreach(var pair in rainMap)
            {
                var Value = pair.Value;
                if (riverBanks.ContainsKey(pair.Key))
                {
                    Value += 0.2f;
                    plusMap[pair.Key] = Value;

                    foreach (var neighor in Hexagon.GetNeighbors(pair.Key).Where(n => !riverBanks.ContainsKey(n) && rainMap.ContainsKey(n)))
                    {
                        plusMap[pair.Key] = rainMap[neighor] + 0.1f;
                    }
                }

                if(!plusMap.ContainsKey(pair.Key))
                {
                    plusMap.Add(pair.Key, Value);
                }
                
            }
            
            var maxValue = plusMap.Max(x => x.Value);

            return plusMap.ToDictionary(k => k.Key, v => v.Value / maxValue);
        }
    }
}