using System.Collections.Generic;

namespace HuangD.Interfaces
{
    public interface IBuffer
    {
        public string title { get; }
        public IEnumerable<IEffect> effects { get; }
    }
}