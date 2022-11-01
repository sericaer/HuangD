using System.Collections;
using System.ComponentModel;
using UnityEngine.UI;

public class ReactiveTest : RxBehaviour<Reactive<int>>
{
    public Text test;

    public override void AssocateData()
    {
        rxUIControl.Binding(dataSource, test);
    }

}