using HuangD.Mods.Interfaces;
using System.Collections.Generic;
using System.Linq;
using static HuangD.Interfaces.IEffect;
using static HuangD.Interfaces.ITreasury;
using static HuangD.Mods.Interfaces.IPopDef.ILiveliHood;

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
                    familys = Enumerable.Range(0, 100).Select(x => $"F{x}"),
                    givens = Enumerable.Range(0, 500).Select(x => $"G{x}")
                },
                provinceDef = new ProvinceDef()
                {
                    names = Enumerable.Range(0, 500).Select(x => $"P{x}"),
                },
                countryNameDef = new CountryNameDef()
                {
                    names = Enumerable.Range(0, 100).Select(x => $"C{x}")
                },
                popDef = new PopDef()
                {
                    liveliHood = new PopDef.LiveliHood
                    {
                        min = 0,
                        max = 100,
                        levels = new Dictionary<string, ILevel>()
                        {
                            {
                                "starve",
                                new PopDef.LiveliHood.Level()
                                {
                                    range = new ILevel.Range()
                                    {
                                        min = 0,
                                        max = 10
                                    },
                                    effects = new EffectDef[]
                                    {
                                        new EffectDef()
                                        {
                                            factor = -0.99,
                                            target = Target.ToPopTax
                                        }
                                    }
                                }
                            },
                            {
                                "struggle",
                                new PopDef.LiveliHood.Level()
                                {
                                    range = new ILevel.Range()
                                    {
                                        min = 20,
                                        max = 30
                                    },
                                    effects = new EffectDef[]
                                        {
                                            new EffectDef()
                                            {
                                                factor = -0.7,
                                                target = Target.ToPopTax
                                            }
                                        }
                                }
                            },
                            {
                                "poor",
                                new PopDef.LiveliHood.Level()
                                {
                                    range = new ILevel.Range()
                                    {
                                        min = 30,
                                        max = 50
                                    },
                                    effects = new EffectDef[]
                                        {
                                            new EffectDef()
                                            {
                                                factor = -0.3,
                                                target = Target.ToPopTax
                                            }
                                        }
                                }
                            },
                            {
                                "midding",
                                new PopDef.LiveliHood.Level()
                                {
                                    range = new ILevel.Range()
                                    {
                                        min = 50,
                                        max = 70
                                    },
                                    effects = new EffectDef[]{ }
                                }
                            },
                            {
                                "secure",
                                new PopDef.LiveliHood.Level()
                                {
                                    range = new ILevel.Range()
                                    {
                                        min = 70,
                                        max = 80
                                    },
                                    effects = new EffectDef[]{ }
                                }
                            },
                            {
                                "prosperous",
                                new PopDef.LiveliHood.Level()
                                {
                                    range = new ILevel.Range()
                                    {
                                        min = 80,
                                        max = 90
                                    },
                                    effects = new EffectDef[]{ }
                                }
                            },
                            {
                                "rish",
                                new PopDef.LiveliHood.Level()
                                {
                                    range = new ILevel.Range()
                                    {
                                        min = 90,
                                        max = 100
                                    },
                                    effects = new EffectDef[]{ }
                                }
                            }
                        }
                    },
                    popTaxLevelBuffs = new Dictionary<CollectLevel, IBufferDef>()
                    {
                        {
                            CollectLevel.VeryLow,
                            new BufferDef()
                            {
                                effects = new IEffectDef[]
                                {
                                    new EffectDef(){ factor = 0, target = Target.ToPopTax },
                                    new EffectDef(){ factor = -0.1, target = Target.ToPopLiveliHoodInc }
                                }
                            }
                        },
                        {
                            CollectLevel.Low,
                            new BufferDef()
                            {
                                effects = new IEffectDef[]
                                {
                                    new EffectDef(){ factor = +0.1, target = Target.ToPopTax },
                                    new EffectDef(){ factor = -0.2, target = Target.ToPopLiveliHoodInc }
                                }
                            }
                        },
                        {
                            CollectLevel.Mid,
                            new BufferDef()
                            {
                                effects = new IEffectDef[]
                                {
                                    new EffectDef(){ factor = +0.2, target = Target.ToPopTax },
                                    new EffectDef(){ factor = -0.4, target = Target.ToPopLiveliHoodInc }
                                }
                            }
                        },
                        {
                            CollectLevel.High,
                            new BufferDef()
                            {
                                effects = new IEffectDef[]
                                {
                                    new EffectDef(){ factor = +0.4, target = Target.ToPopTax },
                                    new EffectDef(){ factor = -0.8, target = Target.ToPopLiveliHoodInc }
                                }
                            }
                        },
                        {
                            CollectLevel.VeryHigh,
                            new BufferDef()
                            {
                                effects = new IEffectDef[]
                                {
                                    new EffectDef(){ factor = +0.8, target = Target.ToPopTax },
                                    new EffectDef(){ factor = -1.2, target = Target.ToPopLiveliHoodInc }
                                }
                            }
                        }
                    },

                }
            }
        };

        static Mod()
        {

        }
    }
}
