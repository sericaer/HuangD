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

            return heightMap.ToDictionary(k => k.Key, v =>
              {
                  if (v.Value < 0.0000001f)
                  {
                      return TerrainType.Water;
                  }
                  else if (v.Value < terrainPercent[TerrainType.Plain] )
                  {
                      return TerrainType.Plain;
                  }
                  else if (v.Value < terrainPercent[TerrainType.Plain] + terrainPercent[TerrainType.Hill])
                  {
                      return TerrainType.Hill;
                  }

                  return TerrainType.Mount;
              });
        }
    }
}
