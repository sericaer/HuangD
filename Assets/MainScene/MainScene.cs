using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Math.TileMap;
using System.Linq;
using HuangD.Sessions;
using HuangD.Interfaces;

public class MainScene : MonoBehaviour
{
    public MapLogic mapLogic;

    // Start is called before the first frame update
    void Awake()
    {
        var mapSize = 100;
        var session = Session.Builder.Build(mapSize, "DEFAULT");

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