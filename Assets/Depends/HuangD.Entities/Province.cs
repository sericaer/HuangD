using HuangD.Interfaces;
using Math.TileMap;
using System;
using System.Collections.Generic;

namespace HuangD.Entities
{
    public partial class Province : IProvince
    {
        public static Func<IProvince, Block> funcGetBlock { get; internal set; }

        public string name { get; set; }

        public (float r, float g, float b) color { get; set; }

        public Block block => funcGetBlock(this);

        public Province(string name, (float, float, float) color)
        {
            this.name = name;
            this.color = color;
        }
    }
}
