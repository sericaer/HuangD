using HuangD.Interfaces;
using HuangD.Mods.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace HuangD.Entities
{
    public partial class Pop
    {
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

            public double baseValue => pop.count.currValue / 1000;

            public IEnumerable<IEffect> effects => pop.buffers.SelectMany(x => x.effects).Where(x => x.target == IEffect.Target.人口税);

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
