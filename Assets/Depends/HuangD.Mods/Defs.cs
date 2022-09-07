using HuangD.Mods.Interfaces;

namespace HuangD.Mods
{
    internal class Defs : IDefs
    {
        public IPersonDef personDef { get; internal set; }
        public ICountryDef countryDef { get; internal set; }

        public IProvinceDef provinceDef { get; internal set; }
    }
}