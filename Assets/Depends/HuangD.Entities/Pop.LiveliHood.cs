using HuangD.Interfaces;
using System.Collections.Generic;
using System.Linq;

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

            public double currValue { get; set; }

            public IEnumerable<IEffect> details => pop.from.buffers.SelectMany(x => x.effects).Where(e => e.target == IEffect.Target.ToPopLiveliHoodInc);

            public double maxValue => pop.def.maxLiveliHood;

            public double minValue => pop.def.minLiveliHood;

            public void OnDaysInc(int year, int month, int day)
            {
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
