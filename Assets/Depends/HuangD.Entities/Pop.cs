using HuangD.Interfaces;
using HuangD.Mods.Interfaces;
using System.Linq;

namespace HuangD.Entities
{
    public partial class Pop : IPop
    {
        public int count => from.cells.Sum(c => c.landInfo.population);

        public IProvince from { get; }

        public IPop.ILiveliHood liveliHood { get; }

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
    }
}
