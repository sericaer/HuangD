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

        public IEnumerable<ITreasury.IIncomeItem> taxItems => _taxItems;
        public IEnumerable<IMilitary.IItem> militaryItems => _militaryItems;
        public IEnumerable<IBuffer> buffers => _buffers;

        private List<ITreasury.IIncomeItem> _taxItems = new List<ITreasury.IIncomeItem>();
        private List<IBuffer> _buffers = new List<IBuffer>();
        private List<IMilitary.IItem> _militaryItems = new List<IMilitary.IItem>();

        public Province(string name, IEnumerable<ICell> cells, IPopDef popDef)
        {
            this.name = name;
            this.cells = cells.ToHashSet();
            
            var pop = new Pop(this, popDef);
            this.pop = pop;

            var popTax = new Pop.PopulationTax(pop);
            popTax.level = CollectLevel.жа;

            _taxItems.Add(popTax);

            var conscript = new Pop.Conscript(pop, IMilitary.CollectLevel.ЕЭ);
            _militaryItems.Add(conscript);
        }

        public void OnDaysInc(int year, int month, int day)
        {
            pop.OnDaysInc(year, month, day);
        }
    }
}
