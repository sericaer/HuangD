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
            人口税,
            生活水平,
            民户增长,
            征兵规模
        }
    }
}
