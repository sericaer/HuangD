using HuangD.Interfaces;

namespace HuangD.Entities
{
    internal partial class Military
    {
        public class Spend : ITreasury.ISpendItem
        {
            public object from => _owner;
            public double currValue => _owner.currValue;

            private Military _owner;

            public Spend(Military military)
            {
                _owner = military;
            }
        }
    }
}