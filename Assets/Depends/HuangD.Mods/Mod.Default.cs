using HuangD.Mods.Interfaces;
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
                    givenNames = Enumerable.Range(0, 500).Select(x=>$"G{x}")
                },
                provinceDef = new ProvinceDef()
                {
                    names = Enumerable.Range(0, 500).Select(x => $"P{x}")
                },
                countryDef = new CountryDef()
                {
                    names = Enumerable.Range(0,100).Select(x=> $"C{x}")
                }
            }
        };

        static Mod()
        {

        }
    }
}
