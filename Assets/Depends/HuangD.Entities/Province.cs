using HuangD.Interfaces;
using Math.TileMap;
using System;
using System.Collections.Generic;
using System.Linq;

namespace HuangD.Entities
{
    public partial class Province : IProvince
    {
        public static Func<IProvince, Block> funcGetBlock { get; internal set; }

        public string name { get; set; }

        public ICell[] cells { get; }
        public IProvince[] neighbors { get; set; }

        public int population => cells.Sum(c => c.landInfo.population);

        //public (float r, float g, float b) color { get; set; }

        //public Block block => funcGetBlock(this);

        public Province(string name, IEnumerable<ICell> cells)
        {
            this.name = name;
            this.cells = cells.ToArray();
        }
    }
}
