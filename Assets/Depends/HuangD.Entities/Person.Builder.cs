using HuangD.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace HuangD.Entities
{
    public partial class Person
    {

        public static class Builder
        {
            internal static IEnumerable<IPerson> Build(int count, string seed)
            {
                return Enumerable.Range(0, count).Select(x => new Person($"F{x}", $"G{x}")).ToArray();
            }
        }

    }
}
