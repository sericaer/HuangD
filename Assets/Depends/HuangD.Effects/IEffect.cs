using HuangD.Interfaces;
using HuangD.Mods.Interfaces;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HuangD.Effects
{
    public class Effect : IEffect
    {
        private IEffectDef def;

        public Effect(IEffectDef def, IBuffer from)
        {
            this.def = def;
            this.from = from;
        }

        public double value => def.factor;
        public IEffect.Target target => def.target;

        public IBuffer from { get; }

    }
}

