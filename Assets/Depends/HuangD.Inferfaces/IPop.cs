using System.Collections.Generic;

namespace HuangD.Interfaces
{
    public interface IPop
    {
        IProvince from { get; }
        public ILiveliHood liveliHood { get; }
        public ICount count { get; }
        public List<IBuffer> buffers { get; }

        public interface ILiveliHood
        {
            public double baseInc { get; }

            public double currValue { get; set; }

            public double surplus { get; }
            public IBuffer level { get; }

            public IEnumerable<IEffect> details { get; }

            void OnDaysInc(int year, int month, int day);
        }

        public interface ICount
        {
            public int currValue { get; }
            public double currInc { get; }

            public IEnumerable<IEffect> details { get; }

            void OnDaysInc(int year, int month, int day);
        }

        void OnDaysInc(int year, int month, int day);
    }
}