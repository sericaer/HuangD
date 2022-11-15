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

                    var key = "CURR_POP_LIVLIHOOD_LEVEL";

                    var buff = pop.buffers.SingleOrDefault(x => (string)x.key == key) as GBuffer;
                    if(buff != null)
                    {
                        if (buff.def == level)
                        {
                            return;
                        }
                        else
                        {
                            pop.buffers.Remove(buff);
                        }
                    }
                    pop.buffers.Add(new GBuffer(key, level));
                }
            }

            private ILevel GetCurrLevel(double currValue)
            {
                return pop.def.liveliHood.levels.Values.SingleOrDefault(x => (int)currValue >= x.range.min && (int)currValue < x.range.max);
            }

            public IEnumerable<IEffect> details => pop.buffers.SelectMany(x => x.effects).Where(e => e.target == IEffect.Target.ToPopLiveliHoodInc);

            public double maxValue => pop.def.liveliHood.max;

            public double minValue => pop.def.liveliHood.min;

            private double _currValue;
            public void OnDaysInc(int year, int month, int day)
            {
                if(day != 1)
                {
                    return;
                }

                currValue += baseInc + details.Sum(x => x.value);

                if(currValue > maxValue)
                {
                    currValue = maxValue;
                }
                if (currValue < minValue)
                {
                    currValue = minValue;
                }
            }
        }
    }
}
