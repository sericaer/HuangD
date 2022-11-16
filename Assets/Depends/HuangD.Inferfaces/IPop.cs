using System.Collections.Generic;

namespace HuangD.Interfaces
{
    public interface IPop
    {
        int count { get; }
        IProvince from { get; }
        public ILiveliHood liveliHood { get; }
        public List<IBuffer> buffers { get; }

        public interface ILiveliHood
        {
            public double baseInc { get; }

            public double currValue { get; set; }

            public IBuffer level { get; }

            public IEnumerable<IEffect> details { get; }

            void OnDaysInc(int year, int month, int day);
        }

        void OnDaysInc(int year, int month, int day);
    }
}