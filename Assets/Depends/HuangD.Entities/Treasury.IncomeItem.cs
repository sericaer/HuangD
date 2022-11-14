//using HuangD.Effects;
//using HuangD.Interfaces;
//using System;
//using System.Collections.Generic;
//using System.Linq;

//namespace HuangD.Entities
//{
//    partial class Treasury
//    {
//        public class IncomeItem : ITreasury.IIncomeItem
//        {
//            public object from { get; }
//            public ITreasury.IIncomeItem.TYPE type { get; }

//            public ITreasury.CollectLevel level
//            {
//                get
//                {
//                    return _level;
//                }
//                set
//                {
//                    if(_level == value)
//                    {
//                        return;
//                    }

//                    _level = value;
//                    onLevelChanged(_level);
//                }
//            }

//            public Action<ITreasury.CollectLevel> onLevelChanged { get; }

//            public double currValue => baseValue * (1 + effects.Sum(x => x.value));

//            public double baseValue => _baseValueFunc();

//            public IEnumerable<IEffect> effects => _effectsFunc().Concat(_effectsByLevel(level)).Where(x=>x.target == type);

//            private ITreasury.CollectLevel _level;
//            private Func<double> _baseValueFunc;
//            private Func<IEnumerable<IEffect>> _effectsFunc;
//            private Func<ITreasury.CollectLevel, IEnumerable<IEffect>> _effectsByLevel;

//            public IncomeItem(
//                ITreasury.IIncomeItem.TYPE type, 
//                object from, 
//                Func<double> baseValueFunc,
//                Func<ITreasury.CollectLevel, IEnumerable<IEffect>> GetEffectsByLevel,
//                Func<IEnumerable<IEffect>> GetOutterEffects,
//                Action<ITreasury.CollectLevel> onLevelChanged
//                )
//            {
//                this.type = type;
//                this.from = from;
//                this._baseValueFunc = baseValueFunc;
//                this._effectsFunc = GetOutterEffects;
//                this._effectsByLevel = GetEffectsByLevel;

//                this.onLevelChanged = onLevelChanged;
//                var rls = _effectsByLevel(level);
//                this.level = ITreasury.CollectLevel.Mid;
//            }

//            public IEnumerable<IEffect> GetEffectsByLevel(ITreasury.CollectLevel level)
//            {
//                return GetEffectsByLevel(level);
//            }
//        }
//    }
//}
