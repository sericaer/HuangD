using HuangD.Interfaces;
using HuangD.Mods.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace HuangD.Entities
{
    public partial class Pop : IPop
    {
        public int count => from.cells.Sum(c => c.landInfo.population);

        public IProvince from { get; }

        public IPop.ILiveliHood liveliHood { get; }

        public IEnumerable<IEffect> effects { get; }

        public List<IBuffer> buffers { get; } = new List<IBuffer>();

        private IPopDef def;

        public Pop(IProvince from, IPopDef def)
        {
            this.from = from;
            this.def = def;
            this.liveliHood = new LiveliHood(this, 30);
        }

        public void OnDaysInc(int year, int month, int day)
        {
            liveliHood.OnDaysInc(year, month, day);
        }

        public class PopulationTax : ITreasury.IIncomeItem
        {
            public ITreasury.CollectLevel level
            {
                get
                {
                    return _level;
                }
                set
                {
                    if (_level == value)
                    {
                        return;
                    }

                    _level = value;

                    pop.buffers.RemoveAll(x => pop.def.popTaxLevelBuffs.ContainsValue(((GBuffer)x).def));

                    pop.buffers.Add(new GBuffer(pop.def.popTaxLevelBuffs[level]));
                }
            }

            public object from => pop;

            public double currValue => baseValue * (System.Math.Max(0, (1 + effects.Sum(x => x.value))));

            public double baseValue => pop.count / 1000;

            public IEnumerable<IEffect> effects => pop.buffers.SelectMany(x => x.effects).Where(x => x.target == IEffect.Target.ToPopTax);

            private readonly Pop pop;
            private ITreasury.CollectLevel _level;

            public IEnumerable<IEffectDef> GetLevelEffects(ITreasury.CollectLevel level)
            {
                return pop.def.popTaxLevelBuffs[level].effects;
            }

            public PopulationTax(Pop pop)
            {
                this.pop = pop;
            }

        }
    }

}
