using HuangD.Mods.Interfaces;
using System.Collections.Generic;
using System.Linq;
using static HuangD.Interfaces.IEffect;
using static HuangD.Interfaces.ITreasury;

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

                provinceDef = new ProvinceDef()
                {
                    names = Enumerable.Range(0, 500).Select(x => $"P{x}"),
                    popTaxLevelBuffs = new Dictionary<CollectLevel, IBufferDef>()
                    {
                        {
                            CollectLevel.VeryLow, 
                            new BufferDef()
                            {
                                effects = new IEffectDef[]
                                {
                                    new EffectDef(){ factor = -0.8, target = Target.ToPopTax }
                                }
                            } 
                        },
                        {
                            CollectLevel.Low,
                            new BufferDef()
                            {
                                effects = new IEffectDef[]
                                {
                                    new EffectDef(){ factor = -0.5, target = Target.ToPopTax }
                                }
                            }
                        },
                        {
                            CollectLevel.Mid,
                            new BufferDef()
                            {
                                effects = new IEffectDef[]
                                {
                                    new EffectDef(){ factor = 0, target = Target.ToPopTax }
                                }
                            }
                        },
                        {
                            CollectLevel.High,
                            new BufferDef()
                            {
                                effects = new IEffectDef[]
                                {
                                    new EffectDef(){ factor = +0.5, target = Target.ToPopTax }
                                }
                            }
                        },
                        {
                            CollectLevel.VeryHigh,
                            new BufferDef()
                            {
                                effects = new IEffectDef[]
                                {
                                    new EffectDef(){ factor = +0.8, target = Target.ToPopTax }
                                }
                            }
                        }
                    }

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
