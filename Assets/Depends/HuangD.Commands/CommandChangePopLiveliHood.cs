using HuangD.Interfaces;
using System.Linq;

namespace HuangD.Commands
{
    public class CommandChangePopLiveliHood : ICommand
    {
        public string key => "ChangePopLiveliHood";

        public string help => "Change Pop LiveliHood";

        public int minArgCount => 2;

        public int maxArgCount => 2;

        public void Exec(ISession session, string[] argvs)
        {
            var targetProvinceName = argvs[0];
            var newValue = int.Parse(argvs[1]);

            var province = session.provinces.Single(x => x.name == targetProvinceName);
            province.pop.liveliHood.currValue = newValue;
        }
    }
}