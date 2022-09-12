namespace HuangD.Interfaces
{
    public interface IOffice
    {
        public string name { get; }
        public bool isLeader { get; }

        public IPerson person { get; }
        public ICountry country { get; }
    }
}