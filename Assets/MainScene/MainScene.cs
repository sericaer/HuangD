using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Math.TileMap;
using System.Linq;
using HuangD.Sessions;
using CommandTerminal;
using HuangD.Commands;
using HuangD.Interfaces;

public class MainScene : MonoBehaviour
{
    public MapLogic mapLogic;
    public CountryPanel countryPanel;

    // Start is called before the first frame update
    void Awake()
    {
        HuangD.Commands.Log.INFO = (info) => Terminal.Log(TerminalLogType.Message, info);
        HuangD.Commands.Log.ERRO = (erro) => Terminal.Log(TerminalLogType.Error, erro);

        var commandMgr = new CommandMgr();

        var mapSize = 50;
        var session = Session.Builder.Build(mapSize, "DEFAULT");

        //emperorPanel.SetEmperor(session.playerCountry.leader);
        countryPanel.SetCountry(session.playerCountry);

        mapLogic.SetMapData(session.map);
        mapLogic.SetProvinces(session.provinces);
        mapLogic.SetCountries(session.countries);

        Terminal.Shell.AddCommand("Echo", new CommandEcho());
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetAxis("Mouse ScrollWheel") > 0)
        {
            mapLogic.ScrollWheel(true);
        }
        if (Input.GetAxis("Mouse ScrollWheel") < 0)
        {
            mapLogic.ScrollWheel(false);
        }
    }
}

public static class CommandShellExtentions
{
    public static void AddCommand(this CommandShell shell, string key, ICommand command)
    {
        System.Action<CommandArg[]> adapter = (args) =>
        {
            command.Exec(args.Select(x => x.String).ToArray());
        };

        shell.AddCommand(key, new CommandInfo() { proc = adapter, help = command.help, min_arg_count = command.minArgCount, max_arg_count = command.maxArgCount });
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

