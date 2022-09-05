using HuangD.Interfaces;
using System;

namespace HuangD.Entities
{
    public partial class Person : IPerson
    {
        internal static Func<IPerson, IOffice> funcGetOffice;

        public string fullName => familyName + givenName;

        public string familyName { get; }

        public string givenName { get; set; }

        public IOffice office => funcGetOffice(this);

        public Person(string familyName, string givenName)
        {
            this.familyName = familyName;
            this.givenName = givenName;
        }

    }
}
