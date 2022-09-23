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
        internal static Dictionary<(int x, int y), float> Build(GRandom random, int high, int width)
        {
            var mapPositions = Enumerable.Range(0, high)
                                         .SelectMany(x => Enumerable.Range(0, width).Select(y => (x, y)));

            var whiteNoiseMap = mapPositions
                        .ToDictionary(k => k, _ => random.getNum(-0.5f, 1.5f));

            var cellularMap = whiteNoiseMap.ToDictionary(k => k.Key, v =>
            {
                var range = Hexagon.GetRange(v.Key,1).Where(r => whiteNoiseMap.ContainsKey(r));
                var average = range.Average(x => whiteNoiseMap[x]);
                if(average > 1f)
                {
                    return random.getNum(0.9f, 1.0f);
                }
                if(average < 0f)
                {
                    return random.getNum(0.0f, 0.1f);
                }
                return average;
            });

            Debug.Log($"0.0-0.1 {cellularMap.Count(v=>v.Value > 0 && v.Value < 0.1) * 100.0 / cellularMap.Count()}");
            Debug.Log($"0.1-0.2 {cellularMap.Count(v => v.Value > 0.1 && v.Value < 0.2) * 100.0 / cellularMap.Count()}");
            Debug.Log($"0.2-0.3 {cellularMap.Count(v => v.Value > 0.2 && v.Value < 0.3) * 100.0 / cellularMap.Count()}");
            Debug.Log($"0.3-0.4 {cellularMap.Count(v => v.Value > 0.3 && v.Value < 0.4) * 100.0 / cellularMap.Count()}");
            Debug.Log($"0.4-0.5 {cellularMap.Count(v => v.Value > 0.4 && v.Value < 0.5) * 100.0 / cellularMap.Count()}");
            Debug.Log($"0.5-0.6 {cellularMap.Count(v => v.Value > 0.5 && v.Value < 0.6) * 100.0 / cellularMap.Count()}");
            Debug.Log($"0.6-0.7 {cellularMap.Count(v => v.Value > 0.6 && v.Value < 0.7) * 100.0 / cellularMap.Count()}");
            Debug.Log($"0.7-0.8 {cellularMap.Count(v => v.Value > 0.7 && v.Value < 0.8) * 100.0 / cellularMap.Count()}");
            Debug.Log($"0.8-0.9 {cellularMap.Count(v => v.Value > 0.8 && v.Value < 0.9) * 100.0 / cellularMap.Count()}");
            Debug.Log($"0.9-1.0 {cellularMap.Count(v => v.Value > 0.9 && v.Value < 1.0) * 100.0 / cellularMap.Count()}");
            return cellularMap;
        }
    }
}
