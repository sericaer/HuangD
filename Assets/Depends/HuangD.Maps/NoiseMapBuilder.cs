using Math.TileMap;
using Maths;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace HuangD.Maps
{
    public static class NoiseMapBuilder
    {
        internal static Dictionary<(int x, int y), float> Build(IEnumerable<(int x, int y)> positions, GRandom random)
        {
            var whiteNoiseMap = positions
                        .ToDictionary(k => k, _ => random.getNum(-0.5f, 1.5f));

            var cellularMap = whiteNoiseMap.ToDictionary(k => k.Key, v =>
            {
                var range = Hexagon.GetRange(v.Key,1).Where(r => whiteNoiseMap.ContainsKey(r));
                var average = range.Average(x => whiteNoiseMap[x]);
                return average;
            });

            var rslt = new Dictionary<(int x, int y), float>();

            var array = cellularMap.OrderBy(x => x.Value).ToArray();
            for(int i=0; i<array.Length; i++)
            {
                rslt.Add(array[i].Key, (i+1) * 1f / array.Length);
            }

            return rslt;
        }
    }
}
