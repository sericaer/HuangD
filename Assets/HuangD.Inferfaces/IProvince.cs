using System.Collections.Generic;

namespace HuangD.Interfaces
{
    public interface IProvince
    {
        public string name { get; }
        public (float r, float g, float b) color { get; }
    }
}