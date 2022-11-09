using HuangD.Interfaces;
using Math.TileMap;
using System;
using System.Collections.Generic;
using System.Linq;
using static HuangD.Interfaces.ITreasury;

namespace HuangD.Entities
{
    public partial class Province : IProvince
    {
        public static Func<IProvince, ICountry> funcGetCountry { get; internal set; }
        public string name { get; set; }
        public IEnumerable<ICell> cells { get; }
        public IEnumerable<IProvince> neighbors { get; set; }

        public ICountry country => funcGetCountry(this);
        public int population => cells.Sum(c => c.landInfo.population);

        public IEnumerable<IIncomeItem> taxItems => _taxItems;

        private List<IIncomeItem> _taxItems = new List<IIncomeItem>();

        public Province(string name, IEnumerable<ICell> cells)
        {
            this.name = name;
            this.cells = cells.ToHashSet();

            _taxItems.Add(new Treasury.IncomeItem(IIncomeItem.TYPE.PopulationTax, this));
        }
    }
}
