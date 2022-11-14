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

    public interface IPop
    {
        int count { get; }

        IProvince from { get; }

        public ILiveliHood liveliHood { get; }

        public interface ILiveliHood
        {
            public double baseInc { get; }

            public double currValue { get; }

            public double maxValue { get; }
            public double minValue { get; }
            public IEnumerable<IEffect> details { get; }

            void OnDaysInc(int year, int month, int day);
        }

        void OnDaysInc(int year, int month, int day);
    }
}