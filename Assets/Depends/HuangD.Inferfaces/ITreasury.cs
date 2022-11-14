using HuangD.Entities;
using HuangD.Mods.Interfaces;
using System;
using System.Collections.Generic;

namespace HuangD.Interfaces
{
    public interface ITreasury
    {
        public double stock { get; }
        public double surplus { get; }

        public double income { get; }
        public double spend { get; }

        public IEnumerable<IIncomeItem> incomeItems { get; }

        public interface IIncomeItem
        {
            public CollectLevel level { get; set; }
            public object from { get; }
            public double currValue { get; }
            public double baseValue { get; }
            public IEnumerable<IEffect> effects { get;}

            public IEnumerable<IEffectDef> GetLevelEffects(CollectLevel level);

        }

        public enum CollectLevel
        {
            VeryLow,
            Low,
            Mid,
            High,
            VeryHigh
        }

        void OnDaysInc(int year, int month, int day);
    }
}


