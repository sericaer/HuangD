using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HuangD.Interfaces
{
    public interface IEffect
    {
        public double value { get; }
        public IBuffer from { get; }

        public Target target { get; }

        public enum Target
        {
            ToPopTax,
            ToPopLiveliHoodInc,
        }
    }
}
