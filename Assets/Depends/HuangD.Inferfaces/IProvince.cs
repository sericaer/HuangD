using Math.TileMap;
using System.Collections.Generic;
using System.Linq;

namespace HuangD.Interfaces
{
    public interface IProvince
    {
        public string name { get; }
        public IEnumerable<ICell> cells { get; }
        public IEnumerable<IProvince> neighbors { get; }
        public IEnumerable<ITreasury.IIncomeItem> taxItems { get; }
        public ICountry country { get; }
        public int population { get; }
    }
}