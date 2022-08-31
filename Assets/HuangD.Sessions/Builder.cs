using HuangD.Interfaces;
using HuangD.Maps;

namespace HuangD.Sessions
{
    public partial class Session
    {
        public static class Builder
        {
            public static ISession Build(int mapSize, string seed)
            {
                var session = new Session();
                session.map = Map.Builder.Build(mapSize, seed);
                return session;
            }
        }
    }
}