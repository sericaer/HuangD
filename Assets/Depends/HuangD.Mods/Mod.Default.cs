using HuangD.Interfaces;
using HuangD.Mods.Interfaces;
using System.Collections.Generic;
using System.Linq;
using static HuangD.Interfaces.IEffect;
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
                                    title = "starve",
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
                                            target = Target.人口税
                                        }
                                    }
                                }
                            },
                            {
                                "struggle",
                                new PopDef.LiveliHood.Level()
                                {
                                    title = "struggle",
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
                                                target = Target.人口税
                                            }
                                        }
                                }
                            },
                            {
                                "poor",
                                new PopDef.LiveliHood.Level()
                                {
                                    title = "poor",
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
                                                target = Target.人口税
                                            }
                                        }
                                }
                            },
                            {
                                "midding",
                                new PopDef.LiveliHood.Level()
                                {
                                    title = "midding",
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
                                    title = "secure",
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
                                    title = "prosperous",
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
                                    title = "rish",
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
                    popTaxLevelBuffs = new Dictionary<ITreasury.CollectLevel, IBufferDef>()
                    {
                        {
                            ITreasury.CollectLevel.极低,
                            new BufferDef()
                            {
                                title = "人口税率极低",
                                effects = new IEffectDef[]
                                {
                                    new EffectDef(){ factor = 0, target = Target.人口税 },
                                    new EffectDef(){ factor = -0.1, target = Target.生活水平 }
                                }
                            }
                        },
                        {
                            ITreasury.CollectLevel.低,
                            new BufferDef()
                            {
                                title = "人口税率低",
                                effects = new IEffectDef[]
                                {
                                    new EffectDef(){ factor = +0.1, target = Target.人口税 },
                                    new EffectDef(){ factor = -0.2, target = Target.生活水平 }
                                }
                            }
                        },
                        {
                            ITreasury.CollectLevel.中,
                            new BufferDef()
                            {
                                title = "人口税率中",
                                effects = new IEffectDef[]
                                {
                                    new EffectDef(){ factor = +0.2, target = Target.人口税 },
                                    new EffectDef(){ factor = -0.4, target = Target.生活水平 }
                                }
                            }
                        },
                        {
                            ITreasury.CollectLevel.高,
                            new BufferDef()
                            {
                                title = "人口税率高",

                                effects = new IEffectDef[]
                                {
                                    new EffectDef(){ factor = +0.4, target = Target.人口税 },
                                    new EffectDef(){ factor = -0.8, target = Target.生活水平 }
                                }
                            }
                        },
                        {
                            ITreasury.CollectLevel.极高,
                            new BufferDef()
                            {
                                title = "人口税率极高",

                                effects = new IEffectDef[]
                                {
                                    new EffectDef(){ factor = +0.8, target = Target.人口税 },
                                    new EffectDef(){ factor = -1.2, target = Target.生活水平 }
                                }
                            }
                        }
                    },
                    ConscriptLevelBuffs = new Dictionary<IMilitary.CollectLevel, IBufferDef>()
                    {
                        {
                            IMilitary.CollectLevel.极低,
                            new BufferDef()
                            {
                                title = "征兵规模极低",

                                effects = new IEffectDef[]
                                {
                                    new EffectDef(){ factor = 0, target = Target.征兵规模 },
                                    new EffectDef(){ factor = -0.1, target = Target.生活水平 }
                                }
                            }
                        },
                        {
                            IMilitary.CollectLevel.低,
                            new BufferDef()
                            {
                                title = "征兵规模低",

                                effects = new IEffectDef[]
                                {
                                    new EffectDef(){ factor = +0.5, target = Target.征兵规模 },
                                    new EffectDef(){ factor = -0.2, target = Target.生活水平 }
                                }
                            }
                        },
                        {
                            IMilitary.CollectLevel.中,
                            new BufferDef()
                            {
                                title = "征兵规模中",

                                effects = new IEffectDef[]
                                {
                                    new EffectDef(){ factor = +1, target = Target.征兵规模 },
                                    new EffectDef(){ factor = -0.4, target = Target.生活水平 }
                                }
                            }
                        },
                        {
                            IMilitary.CollectLevel.高,
                            new BufferDef()
                            {
                                title = "征兵规模高",

                                effects = new IEffectDef[]
                                {
                                    new EffectDef(){ factor = +2, target = Target.征兵规模 },
                                    new EffectDef(){ factor = -0.8, target = Target.生活水平 }
                                }
                            }
                        },
                        {
                            IMilitary.CollectLevel.极高,
                            new BufferDef()
                            {
                                title = "征兵规模极高",

                                effects = new IEffectDef[]
                                {
                                    new EffectDef(){ factor = +3, target = Target.征兵规模 },
                                    new EffectDef(){ factor = -1.2, target = Target.生活水平 }
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
