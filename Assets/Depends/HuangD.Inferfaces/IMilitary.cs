using HuangD.Mods.Interfaces;
using System.Collections.Generic;

namespace HuangD.Interfaces
{
    public interface IMilitary
    {
        public int currValue { get; }
        public int maxValue { get; }
        public double incValue { get; }

        public IEnumerable<IItem> items { get;}

        public ITreasury.ISpendItem spend { get; }

        public enum CollectLevel
        {
            极低,
            低,
            中,
            高,
            极高
        }

        public interface IItem
        {
            public int currValue { get; set; }
            public int maxValue { get; }
            public double incValue { get; }

            public CollectLevel level { get; set; }
            public object from { get; }

            public IEnumerable<IEffect> effects { get; }

            public IEnumerable<IEffectDef> GetLevelEffects(CollectLevel level);
        }

        void OnDaysInc(int year, int month, int day);
    }
}