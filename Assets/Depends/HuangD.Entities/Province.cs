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
        public int population => cells.Sum(c => c.landInfo.population);

        public IEnumerable<IIncomeItem> taxItems => _taxItems;

        public IEnumerable<IBuffer> buffers => _buffers;

        private List<IIncomeItem> _taxItems = new List<IIncomeItem>();
        private List<IBuffer> _buffers = new List<IBuffer>();

        public Province(string name, IEnumerable<ICell> cells, IPopDef popDef)
        {
            this.name = name;
            this.cells = cells.ToHashSet();
            this.pop = new Pop(this, popDef);

            var taxItem = new Treasury.IncomeItem(
                IIncomeItem.TYPE.PopulationTax, 
                this,
                () => pop.count / 1000,
                () => buffers.SelectMany(x=>x.effects).Where(x=>x.target == IEffect.Target.ToPopTax),
                (level) =>
                {
                    var key = "CURR_POP_TAX_LEVEL";
                    _buffers.RemoveAll(x => (string)x.key == key);

                    _buffers.Add(new GBuffer(key, def.popTaxLevelBuffs[level]));
                }
             );


            _taxItems.Add(taxItem);
        }

        public void OnDaysInc(int year, int month, int day)
        {
            pop.OnDaysInc(year, month, day);
        }
    }
}
