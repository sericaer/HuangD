using HuangD.Interfaces;
using HuangD.Mods.Interfaces;
using System.Collections.Generic;
using System.Linq;
using static HuangD.Mods.Interfaces.IPopDef.ILiveliHood;

namespace HuangD.Entities
{
    public partial class Pop
    {
        public class LiveliHood : IPop.ILiveliHood
        {

            private Pop pop;

            public LiveliHood(Pop pop, double initValue)
            {
                this.pop = pop;
                this.currValue = initValue;
            }

            public double baseInc => 1;

            public double currValue
            {
                get
                {
                    return _currValue;
                }
                set
                {
                    _currValue = value;

                    var level = GetCurrLevel(currValue);

                    pop.buffers.RemoveAll(x => pop.def.liveliHood.levels.ContainsValue(((GBuffer)x).def as ILevel));

                    pop.buffers.Add(new GBuffer(level));
                }
            }

            private ILevel GetCurrLevel(double currValue)
            {
                return pop.def.liveliHood.levels.Values.SingleOrDefault(x => (int)currValue >= x.range.min && (int)currValue < x.range.max);
            }

            public IEnumerable<IEffect> details => pop.buffers.SelectMany(x => x.effects).Where(e => e.target == IEffect.Target.ToPopLiveliHoodInc);

            public IBuffer level => pop.buffers.SingleOrDefault(x => pop.def.liveliHood.levels.ContainsValue(((GBuffer)x).def as ILevel));

            public double surplus => baseInc + details.Sum(x => x.value);

            private double _currValue;

            public void OnDaysInc(int year, int month, int day)
            {
                if(day != 1)
                {
                    return;
                }

                var newValue = currValue + surplus;

                if (newValue > pop.def.liveliHood.max)
                {
                    currValue = pop.def.liveliHood.max;
                }
                else if (newValue < pop.def.liveliHood.min)
                {
                    currValue = pop.def.liveliHood.min;
                }
                else
                {
                    currValue = newValue;
                }
            }
        }
    }
}
