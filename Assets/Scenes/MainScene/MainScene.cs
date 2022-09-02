using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Math.TileMap;
using HuangD.Sessions;
using CommandTerminal;
using HuangD.Commands;

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
        foreach(var command in commandMgr.all)
        {
            Terminal.Shell.AddCommand(command);
        }

        var mapSize = 100;
        var session = Session.Builder.Build(mapSize, "DEFAULT");

        //emperorPanel.SetEmperor(session.playerCountry.leader);
        countryPanel.SetCountry(session.playerCountry);

        mapLogic.SetMapData(session.map);
        mapLogic.SetProvinces(session.provinces);
        mapLogic.SetCountries(session.countries);


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

