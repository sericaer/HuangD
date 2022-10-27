using Math.TileMap;
using System.Collections.Generic;

namespace HuangD.Interfaces
{
    public interface IProvince
    {
        public string name { get; }
        public IEnumerable<ICell> cells { get; }
        public IEnumerable<IProvince> neighbors { get; }

        public ICountry country { get; }
        public int population { get; }
    }
}