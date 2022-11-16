using HuangD.Interfaces;
using HuangD.Mods.Interfaces;
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
        public IPop pop { get; }
        public ICountry country => funcGetCountry(this);

        public IEnumerable<IIncomeItem> taxItems => _taxItems;

        public IEnumerable<IBuffer> buffers => _buffers;

        private List<IIncomeItem> _taxItems = new List<IIncomeItem>();
        private List<IBuffer> _buffers = new List<IBuffer>();

        public Province(string name, IEnumerable<ICell> cells, IPopDef popDef)
        {
            this.name = name;
            this.cells = cells.ToHashSet();
            
            var pop = new Pop(this, popDef);
            this.pop = pop;

            var popTax = new Pop.PopulationTax(pop);
            popTax.level = CollectLevel.жа;

            _taxItems.Add(popTax);
        }

        public void OnDaysInc(int year, int month, int day)
        {
            pop.OnDaysInc(year, month, day);
        }
    }
}
