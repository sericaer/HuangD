using HuangD.Interfaces;
using HuangD.Mods.Interfaces;
using Maths;
using System.Collections.Generic;

namespace HuangD.Entities
{
    public partial class Person
    {
        private static IPersonNameDef def;

        public static class Builder
        {
            internal static IEnumerable<IPerson> Build(int count, string seed, IPersonNameDef personDef)
            {
                var random = new GRandom(seed);

                Person.def = personDef;

                var persons = new List<IPerson>();
                for(int i=0; i<count; i++)
                {
                    persons.Add(new Person(random.Get(Person.def.familys), random.Get(Person.def.givens)));
                }

                return persons;
            }
        }

    }
}
