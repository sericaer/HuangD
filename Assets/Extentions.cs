using System.Linq;
using CommandTerminal;
using HuangD.Interfaces;

public static class CommandShellExtentions
{
    public static void AddCommand(this CommandShell shell, ICommand command)
    {
        System.Action<CommandArg[]> adapter = (args) =>
        {
            var commandClone = System.Activator.CreateInstance(command.GetType()) as ICommand;
            commandClone.Exec(Facade.session, args.Select(x => x.String).ToArray());
        };

        shell.AddCommand(command.key, new CommandInfo() { proc = adapter, help = command.help, min_arg_count = command.minArgCount, max_arg_count = command.maxArgCount });
    }

    public static void AddCommand(this CommandShell shell, string key, System.Action<string[]> command)
    {
        System.Action<CommandArg[]> adapter = (args) =>
        {
            command.Invoke(args.Select(x => x.String).ToArray());
        };

        shell.AddCommand(key, new CommandInfo() { proc = adapter, help = "this is demo", min_arg_count = 1, max_arg_count = 10 });
    }
}

