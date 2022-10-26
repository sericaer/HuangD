using Math.TileMap;
using System.Collections.Generic;

namespace HuangD.Interfaces
{
    public interface IProvince
    {
        public string name { get; }
        public ICell[] cells { get; }
        public IProvince[] neighbors { get; }

        public int population { get; }
    }
}