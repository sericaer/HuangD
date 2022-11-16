using HuangD.Mods.Interfaces;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using static HuangD.Mods.Interfaces.IPopDef.ILiveliHood;

namespace HuangD.Mods
{
    internal partial class PopDef
    {
        public class LiveliHood : IPopDef.ILiveliHood
        {
            public double max { get; set; }
            public double min { get; set; }
            public Dictionary<string, IPopDef.ILiveliHood.ILevel> levels { get; set; }

            public LiveliHood()
            {

            }

            [JsonConstructor]
            public LiveliHood(double min, double max, Dictionary<string, Level> levels)
            {
                this.min = min;
                this.max = max;

                foreach(var pair in levels)
                {
                    if(pair.Value.title == null)
                    {
                        pair.Value.title = pair.Key;
                    }
                }

                this.levels = levels.ToDictionary(p => p.Key, p => (ILevel)p.Value);
            }

            public class Level : BufferDef, ILevel
            {
                public ILevel.Range range { get ; set ; }
            }
        }
    }
}
