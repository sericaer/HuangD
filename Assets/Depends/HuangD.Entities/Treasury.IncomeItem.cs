using HuangD.Effects;
using HuangD.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace HuangD.Entities
{
    partial class Treasury
    {
        public class IncomeItem : ITreasury.IIncomeItem
        {
            public object from { get; }
            public ITreasury.IIncomeItem.TYPE type { get; }

            public ITreasury.CollectLevel level
            {
                get
                {
                    return _level;
                }
                set
                {
                    if(_level == value)
                    {
                        return;
                    }

                    _level = value;
                    onLevelChanged(_level);
                }
            }

            public Action<ITreasury.CollectLevel> onLevelChanged { get; }

            public double currValue => baseValue * (1 + effects.Sum(x => x.value));

            public double baseValue => _baseValueFunc();

            public IEnumerable<IEffect> effects => _effectsFunc();

            private ITreasury.CollectLevel _level;
            private Func<double> _baseValueFunc;
            private Func<IEnumerable<IEffect>> _effectsFunc;

            public IncomeItem(
                ITreasury.IIncomeItem.TYPE type, 
                object from, 
                Func<double> baseValueFunc,
                Func<IEnumerable<IEffect>> effectsFunc,
                Action<ITreasury.CollectLevel> onLevelChanged
                )
            {
                this.type = type;
                this.from = from;
                this._baseValueFunc = baseValueFunc;
                this._effectsFunc = effectsFunc;
                this.onLevelChanged = onLevelChanged;

                this.level = ITreasury.CollectLevel.Mid;
            }

            private double CalcPoxTax(IProvince province)
            {
                var totalEffectValue = province.buffers.SelectMany(b=>b.effects).Where(x=>x.target == IEffect.Target.ToPopTax).Sum(x => x.value);
                return province.pop.count / 1000.0 * (1+totalEffectValue);
            }
        }
    }
}
