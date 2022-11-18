using HuangD.Interfaces;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopPanel : UIBehaviour<IPop>
{
    public Text population;
    public Text populationIncFlag;

    public Text liveliHood;
    public Text liveliHoodIncFlag;

    public Text populationDetail;
    public NumberText populationInc;

    public Text liveliHoodDetail;
    public Text liveliHoodValue;
    public NumberText liveliHoodInc;

    protected override void AssocDataSource()
    {
        Bind(pop => pop.count.currValue, population);
        Bind(pop => pop.count.currInc > 0 ? "¡ü" : "¡ý", populationIncFlag);

        Bind(pop => pop.liveliHood.level.title, liveliHood);
        Bind(pop => pop.liveliHood.surplus > 0 ? "¡ü" : "¡ý", liveliHoodIncFlag);

        Bind(pop => pop.count.currValue, populationDetail);
        Bind(pop => pop.count.currInc, liveliHoodInc);

        Bind(pop => pop.liveliHood.level.title, liveliHoodDetail);
        Bind(pop => pop.liveliHood.currValue, liveliHoodValue);
        Bind(pop => pop.liveliHood.surplus, liveliHoodInc);
    }
}
