using HuangD.Interfaces;
using HuangD.Mods.Inferfaces;

public static class Facade
{
    public static ISession session { get; set; }
    public static IMod mod { get; set; }
}
