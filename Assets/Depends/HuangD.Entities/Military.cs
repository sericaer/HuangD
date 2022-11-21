using HuangD.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace HuangD.Entities
{
    internal partial class Military : IMilitary
    {
        public int currValue => items.Sum(x => x.currValue);

        public int maxValue => items.Sum(x => x.maxValue);

        public double incValue => items.Sum(x => x.incValue);

        public IEnumerable<IMilitary.IItem> items { get; }

        private ICountry owner { get; }

        public Military(ICountry country)
        {
            owner = country;
            items = country.provinces.SelectMany(x => x.militaryItems);
        }

        public void OnDaysInc(int year, int month, int day)
        {
            if(day == 30)
            {
                foreach (var item in items)
                {
                    var newValue = item.currValue + incValue;
                    if (newValue > item.maxValue)
                    {
                        newValue = item.maxValue;
                    }
                    if (newValue < 0)
                    {
                        newValue = 0;
                    }

                    item.currValue = (int)newValue;
                }
            }
        }
    }
}