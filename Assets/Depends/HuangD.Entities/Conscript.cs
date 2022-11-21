using HuangD.Interfaces;
using HuangD.Mods.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace HuangD.Entities
{
    public partial class Pop
    {
        internal class Conscript : IMilitary.IItem
        {
            public int currValue { get; set; }

            public int maxValue => (int)(pop.count.currValue / 3 * effects.Sum(x => x.value));

            public double incValue => maxValue / 6;

            public IMilitary.CollectLevel level
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

                    var buffDef = pop.def.ConscriptLevelBuffs[level];

                    pop.buffers.RemoveAll(x => pop.def.ConscriptLevelBuffs.ContainsValue(((GBuffer)x).def));

                    pop.buffers.Add(new GBuffer(pop.def.ConscriptLevelBuffs[level]));
                }
            }

            public object from => pop;

            public IEnumerable<IEffect> effects => pop.buffers.SelectMany(x => x.effects).Where(x => x.target == IEffect.Target.征兵规模);

            private Pop pop;
            private IMilitary.CollectLevel _level;

            public Conscript(Pop pop, IMilitary.CollectLevel level)
            {
                this.pop = pop;
                this.level = level;

                currValue = maxValue / 2;
            }

            public IEnumerable<IEffectDef> GetLevelEffects(IMilitary.CollectLevel level)
            {
                return pop.def.ConscriptLevelBuffs[level].effects;
            }
        }
    }

}