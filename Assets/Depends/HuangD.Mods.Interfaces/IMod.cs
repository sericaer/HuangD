using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HuangD.Mods.Interfaces
{
    public interface IMod
    {
        public IDefs defs { get; }
    }

    public interface IDefs
    {
        public IPersonNameDef personNameDef { get; }
        public ICountryNameDef countryNameDef { get; }
        public IProvinceDef provinceDef { get; }
        public IPopDef popDef { get; }
    }

    public interface IPersonNameDef
    {
        public IEnumerable<string> familys { get; }
        public IEnumerable<string> givens { get; }
    }
}
