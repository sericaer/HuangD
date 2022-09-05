namespace HuangD.Interfaces
{
    public interface ICommand
    {
        string key { get; }
        string help { get; }
        int minArgCount { get; }
        int maxArgCount { get; }

        void Exec(ISession session, string[] argc);
    }
}


