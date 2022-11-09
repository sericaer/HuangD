using HuangD.Interfaces;
using System;
using System.Collections.Generic;

namespace HuangD.Entities
{
    partial class Treasury
    {
        public class IncomeItem : ITreasury.IIncomeItem
        {
            public object from { get; }
            public ITreasury.IIncomeItem.TYPE type { get; }

            public ITreasury.CollectLevel level { get; set; }

            public IncomeItem(ITreasury.IIncomeItem.TYPE type, object from)
            {
                this.type = type;
                this.from = from;
            }

            public double GetValue()
            {
                switch(type)
                {
                    case ITreasury.IIncomeItem.TYPE.PopulationTax:
                        return CalcPoxTax(from as IProvince);
                    default:
                        throw new System.Exception();
                }
            }

            private double CalcPoxTax(IProvince province)
            {
                return province.population / 1000.0;
            }
        }
    }
}
