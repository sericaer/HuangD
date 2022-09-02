using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Math.TileMap;
using System.Linq;
using HuangD.Sessions;

public class MainScene : MonoBehaviour
{
    public MapLogic mapLogic;
    public CountryPanel countryPanel;

    // Start is called before the first frame update
    void Awake()
    {
        var mapSize = 50;
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
