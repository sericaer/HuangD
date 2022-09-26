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
        internal static Dictionary<(int x, int y), TerrainType> Build(Dictionary<(int x, int y), float> heightMap)
        {
            return heightMap.ToDictionary(k => k.Key, v =>
              {
                  if (v.Value < 0)
                  {
                      return TerrainType.Water;
                  }
                  else if (v.Value < 0.3)
                  {
                      return TerrainType.Plain;
                  }
                  else if (v.Value < 0.6)
                  {
                      return TerrainType.Hill;
                  }
                  return TerrainType.Mount;
              });
        }
    }
}
