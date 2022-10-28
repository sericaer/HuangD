using HuangD.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace HuangD.Commands
{
    public class CommandMgr
    {
        public IEnumerable<ICommand> all { get; }

        public CommandMgr()
        {
            all = Assembly.GetExecutingAssembly().GetTypes()
                .Where(t => t.Namespace == "HuangD.Commands" && typeof(ICommand).IsAssignableFrom(t))
                .Select(t => Activator.CreateInstance(t) as ICommand)
                .ToArray();
        }

        public T GetCmd<T>() where T:ICommand
        {
            return all.OfType<T>().SingleOrDefault();
        }
    }

    public static class Log
    {
        public static Action<string> INFO;
        public static Action<string> ERRO;
    }
}

