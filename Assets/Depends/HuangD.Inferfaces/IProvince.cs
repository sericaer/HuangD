using HuangD.Interfaces;
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
        public IPop pop { get; }
        public IEnumerable<IBuffer> buffers { get; }

        void OnDaysInc(int year, int month, int day);
    }
}