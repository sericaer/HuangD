using HuangD.Interfaces;
using Math.TileMap;
using Maths;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace HuangD.Maps
{
    static class TerrainMapBuilder
    {
        internal static Dictionary<(int x, int y), TerrainType> Build(Dictionary<(int x, int y), float> heightMap, Dictionary<TerrainType, int> terrainFactors)
        {
            var terrainPercent = terrainFactors.ToDictionary(p => p.Key, p => p.Value * 1f / terrainFactors.Values.Sum());

            var rslt = new Dictionary<(int x, int y), TerrainType>();

            foreach(var pair in heightMap.Where(x=>x.Value < 0.00000001f))
            {
                rslt.Add(pair.Key, TerrainType.Water);
            }

            var landPositions = heightMap.Keys.Except(rslt.Keys).OrderBy(k => heightMap[k]).ToArray();
            for(int i=0; i< landPositions.Length; i++)
            {
                var pos = landPositions[i];
                var percent = i * 1.0f / landPositions.Length;

                if (percent < terrainPercent[TerrainType.Plain])
                {
                    rslt.Add(pos, TerrainType.Plain);
                }
                else if (percent < terrainPercent[TerrainType.Plain] + terrainPercent[TerrainType.Hill])
                {
                    rslt.Add(pos, TerrainType.Hill);
                }
                else
                {
                    rslt.Add(pos, TerrainType.Mount);
                }
            }

            return rslt;
        }
    }
}
