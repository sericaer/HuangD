using HuangD.Interfaces;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopPanel : UIBehaviour<IPop>
{
    public Text population;
    public Text liveliHood;
    public Text liveliHoodIncFlag;

    public Text populationDetail;
    public Text liveliHoodDetail;
    public Text liveliHoodValue;
    public NumberText liveliHoodInc;

    protected override void AssocDataSource()
    {
        Bind(pop => pop.count, population);
        Bind(pop => pop.liveliHood.level.title, liveliHood);
        Bind(pop => pop.liveliHood.surplus > 0 ? "¡ü" : "¡ý", liveliHoodIncFlag);

        Bind(pop => pop.count, populationDetail);
        Bind(pop => pop.liveliHood.level.title, liveliHoodDetail);
        Bind(pop => pop.liveliHood.currValue, liveliHoodValue);
        Bind(pop => pop.liveliHood.surplus, liveliHoodInc);
    }
}
