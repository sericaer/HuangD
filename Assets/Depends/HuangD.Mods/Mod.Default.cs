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
                personNameDef = new PersonNameDef()
                {
                    familys = Enumerable.Range(0,100).Select(x=>$"F{x}"),
                    givens = Enumerable.Range(0, 500).Select(x=>$"G{x}")
                },
                provinceNameDef = new ProvinceNameDef()
                {
                    names = Enumerable.Range(0, 500).Select(x => $"P{x}")
                },
                countryNameDef = new CountryNameDef()
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
