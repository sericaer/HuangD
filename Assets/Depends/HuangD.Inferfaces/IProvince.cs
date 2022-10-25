using Math.TileMap;
using System.Collections.Generic;

namespace HuangD.Interfaces
{
    public interface IProvince
    {
        public string name { get; }
        //public (float r, float g, float b) color { get; }

        public ICell[] cells { get; }

        //public IProvince[] neighors { get; }
        //public Block block { get; }
    }
}