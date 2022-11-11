using System.Collections.Generic;

namespace HuangD.Interfaces
{
    public interface IBuffer
    {
        public string key { get; }
        public IEnumerable<IEffect> effects { get; }
    }
}