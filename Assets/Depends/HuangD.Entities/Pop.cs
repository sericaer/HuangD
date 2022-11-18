using HuangD.Interfaces;
using HuangD.Mods.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace HuangD.Entities
{
    public partial class Pop : IPop
    {
        public IProvince from { get; }

        public IPop.ILiveliHood liveliHood { get; }

        public IPop.ICount count { get; }

        public IEnumerable<IEffect> effects { get; }

        public List<IBuffer> buffers { get; } = new List<IBuffer>();

        private IPopDef def;

        public Pop(IProvince from, IPopDef def)
        {
            this.from = from;
            this.def = def;
            this.liveliHood = new LiveliHood(this, 30);
            this.count = new Count(this);
        }

        public void OnDaysInc(int year, int month, int day)
        {
            liveliHood.OnDaysInc(year, month, day);
            count.OnDaysInc(year, month, day);
        }
    }

}
