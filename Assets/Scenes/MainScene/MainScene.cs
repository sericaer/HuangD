using CommandTerminal;
using HuangD.Commands;
using HuangD.Mods;
using HuangD.Sessions;
using UnityEngine;

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
        foreach (var command in commandMgr.all)
        {
            Terminal.Shell.AddCommand(command);
        }

        //var mapSize = 80;

        //Facade.session = Session.Builder.Build(mapSize, "DEFAULT", Facade.mod.defs);

        //emperorPanel.SetEmperor(session.playerCountry.leader);
        //countryPanel.SetCountry(session.playerCountry);

        mapLogic.SetMapData(Facade.session.map);
        //mapLogic.SetProvinces(Facade.session.provinces);
        //mapLogic.SetCountries(Facade.session.countries);


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

