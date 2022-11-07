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

        public double income => _incomeDetail.Sum(x=> x.GetValue());

        public double spend { get; }

        private ICountry owner { get; }

        private List<IncomeItem> _incomeDetail = new List<IncomeItem>();

        private double? _stock;

        public Treasury(ICountry country)
        {
            owner = country;

            _incomeDetail.AddRange(owner.provinces.Select(prov => new IncomeItem(ITreasury.IIncomeItem.TYPE.PopulationTax, prov)));
        }
    }
}
