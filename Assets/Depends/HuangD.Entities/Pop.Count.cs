using HuangD.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace HuangD.Entities
{
    public partial class Pop
    {
        class Count : IPop.ICount
        {
            private Pop owner;

            public int currValue => owner.from.cells.Sum(c => c.landInfo.population);
            public double currInc => details.Sum(x => x.value);

            public IEnumerable<IEffect> details => owner.buffers.SelectMany(x => x.effects).Where(e => e.target == IEffect.Target.民户增长);

            public Count(Pop pop)
            {
                this.owner = pop;
            }

            public void OnDaysInc(int year, int month, int day)
            {
                if (day != 1)
                {
                    return;
                }

                foreach(var cell in owner.from.cells)
                {
                    cell.landInfo.PopInc(currInc);
                }
            }
        }
    }

}
