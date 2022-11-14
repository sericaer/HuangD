using HuangD.Mods.Interfaces;
using Newtonsoft.Json;
using System.IO;

namespace HuangD.Mods
{
    internal class Defs : IDefs
    {
        public IPersonNameDef personNameDef { get; internal set; }
        public ICountryNameDef countryNameDef { get; internal set; }

        public IProvinceDef provinceDef { get; internal set; }

        public IPopDef popDef { get; internal set; }

        public Defs(ModFileSystem fileSystem)
        {
            personNameDef = JsonConvert.DeserializeObject<PersonNameDef>(fileSystem.personNames);
            countryNameDef = JsonConvert.DeserializeObject<CountryNameDef>(fileSystem.countryNames);

            //provinceDef = ProvinceDef.Builder.Build(Path.Combine(fileSystem.modPath, "Defines", "Province")); 

            provinceDef = JsonConvert.DeserializeObject<ProvinceDef>(fileSystem.provinceNames);

            popDef = PopDef.Builder.Build(fileSystem);
        }

        internal Defs()
        {

        }
    }
}