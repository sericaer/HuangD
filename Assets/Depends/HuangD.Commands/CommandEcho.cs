using HuangD.Interfaces;

namespace HuangD.Commands
{
    public class CommandEcho : ICommand
    {
        public string key => "Echo";

        public string help => "这是一个回声";

        public int minArgCount => 1;

        public int maxArgCount => 2;

        public void Exec(ISession session, string[] argc)
        {
            Log.INFO(string.Join(",", argc));
        }
    }
}