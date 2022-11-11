using HuangD.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace HuangD.Entities
{
    partial class Treasury : ITreasury
    {

        public double stock
        {
            get
            {
                if(_stock == null)
                {
                    _stock = surplus * 6;
                }

                return _stock.Value;
            }
            set
            {
                _stock = value;
            }
        }

        public double surplus => income - spend;

        public double income => incomeItems.Sum(x=> x.currValue);
        public IEnumerable<ITreasury.IIncomeItem> incomeItems { get; }

        public double spend { get; }

        private ICountry owner { get; }

        private double? _stock;

        public Treasury(ICountry country)
        {
            owner = country;

            incomeItems = owner.provinces.SelectMany(prov => prov.taxItems);
        }

        public void OnDaysInc(int year, int month, int day)
        {
            if(day == 30)
            {
                stock += surplus;
            }
        }
    }
}
