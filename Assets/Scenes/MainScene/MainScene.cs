using CommandTerminal;
using HuangD.Commands;
using HuangD.Mods;
using HuangD.Sessions;
using UnityEngine;

public class MainScene : MonoBehaviour
{
    public MapCanvas mapCanvas;
    public CountryPanel countryPanel;
    public DatePanel datePanel;
    public TreasuryPanel treasuryPanel;
    public void OnTimeElapse()
    {
        Facade.session.DaysInc();
    }

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

        commandMgr.GetCmd<CommandChangePlayCountry>().OnChangedListeners.Clear();
        commandMgr.GetCmd<CommandChangePlayCountry>().OnChangedListeners.Add((country)=> 
        {
            mapCanvas.moveCameraTo(country);
        });

        UIBehaviourBase.AssocTooltip = (obj, func) =>
        {
            obj.GetComponent<DynamicToolTip>().GenerateBody = func;
        };

        //emperorPanel.SetEmperor(session.playerCountry.leader);
        //countryPanel.SetCountry(session.playerCountry);

        mapCanvas.SetMapData(Facade.session);

        datePanel.dataSource = Facade.session.date;
        treasuryPanel.dataSource = Facade.session.playerCountry.treasury;

        //mapLogic.SetProvinces(Facade.session.provinces);
        //mapLogic.SetCountries(Facade.session.countries);


    }

    // Update is called once per frame
    void Update()
    {
        //if (Input.GetAxis("Mouse ScrollWheel") > 0)
        //{
        //    mapLogic.ScrollWheel(true);
        //}
        //if (Input.GetAxis("Mouse ScrollWheel") < 0)
        //{
        //    mapLogic.ScrollWheel(false);
        //}
    }
}
