using HuangD.Mods.Inferfaces;
using System.Linq;

namespace HuangD.Mods
{
    public partial class Mod
    {
        public static IMod Default { get; } = new Mod()
        {
            defs = new Defs()
            {
                personDef = new PersonDef()
                {
                    familyNames = Enumerable.Range(0,100).Select(x=>$"F{x}"),
                    givenNames = Enumerable.Range(0, 100).Select(x=>$"G{x}")
                }
            }
        };

        static Mod()
        {

        }
    }
}
