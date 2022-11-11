using CommandTerminal;
using HuangD.Commands;
using HuangD.Mods;
using HuangD.Sessions;
using System;
using UnityEngine;

public class MainScene : MonoBehaviour
{
    public MapCanvas mapCanvas;
    public CountryPanel countryPanel;
    public DatePanel datePanel;

    public void OnTimeElapse()
    {
        Facade.session.DaysInc();
    }

    // Start is called before the first frame update
    void Awake()
    {
        InitializeLog();
        InitializeCommand();
        InitializeTooltip();

        mapCanvas.SetMapData(Facade.session);

        datePanel.dataSource = Facade.session.date;

        FixedUpdate();
    }

    private static void InitializeTooltip()
    {
        UIBehaviourBase.AssocTooltip = (obj, func) =>
        {
            obj.GetComponent<DynamicToolTip>().GenerateBody = func;
        };
    }

    private static void InitializeLog()
    {
        HuangD.Commands.Log.INFO = (info) => Terminal.Log(TerminalLogType.Message, info);
        HuangD.Commands.Log.ERRO = (erro) => Terminal.Log(TerminalLogType.Error, erro);
    }

    private static void InitializeCommand()
    {
        var commandMgr = new CommandMgr();
        foreach (var command in commandMgr.all)
        {
            Terminal.Shell.AddCommand(command);
        }
    }

    private void FixedUpdate()
    {
        countryPanel.country = Facade.session.playerCountry;
    }
}
