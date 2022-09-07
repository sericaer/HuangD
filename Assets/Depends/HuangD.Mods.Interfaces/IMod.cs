using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HuangD.Mods.Inferfaces
{
    public interface IMod
    {
        public IDefs defs { get; }
    }

    public interface IDefs
    {
        public IPersonDef personDef { get; }
    }

    public interface IPersonDef
    {
        public IEnumerable<string> familyNames { get; }
        public IEnumerable<string> givenNames { get; }
    }
}
