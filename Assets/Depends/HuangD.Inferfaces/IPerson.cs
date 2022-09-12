namespace HuangD.Interfaces
{
    public interface IPerson
    {
        public string familyName { get; }
        public string givenName { get; }
        public string fullName { get; }

        public IOffice office { get; }
    }
}