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
            var plusMap = rainMap.Select(p =>
            {
                var Value = p.Value;
                if(riverBanks.ContainsKey(p.Key))
                {
                    Value += 0.6f;

                    foreach(var neighor in Hexagon.GetNeighbors(p.Key).Where(n=> !riverBanks.ContainsKey(n) && rainMap.ContainsKey(n)))
                    {
                        Value += 0.3f;
                    }

                }

                return (Key:p.Key, Value:Value);
            });

            var maxValue = plusMap.Max(x => x.Value);

            return plusMap.ToDictionary(k => k.Key, v => v.Value / maxValue);
        }
    }
}