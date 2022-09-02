namespace HuangD.Interfaces
{
    public interface ICommand
    {
        string help { get; }
        int minArgCount { get; }
        int maxArgCount { get; }

        void Exec(string[] argc);
    }
}


