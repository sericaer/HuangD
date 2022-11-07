using HuangD.Entities;

namespace HuangD.Interfaces
{
    public interface ITreasury
    {
        public double stock { get; }
        public double surplus { get; }

        public double income { get; }
        public double spend { get; }


        public interface IIncomeItem
        {
            public enum TYPE
            {
                PopulationTax
            }

            public TYPE type { get; }
            public object from { get; }

            public double GetValue();
        }
    }
}


