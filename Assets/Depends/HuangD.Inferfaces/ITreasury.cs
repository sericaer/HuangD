using HuangD.Entities;
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
            public enum TYPE
            {
                PopulationTax
            }

            public CollectLevel level { get; set; }

            public Action<CollectLevel> onLevelChanged { get; }
            public TYPE type { get; }
            public object from { get; }
            public double currValue { get; }
            public double baseValue { get; }
            public IEnumerable<IEffect> effects { get;}
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


