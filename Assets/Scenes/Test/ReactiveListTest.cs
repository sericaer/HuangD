using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UI;

public class ReactiveListTest : RxListBehaviour<Data>
{
    public ReactiveListItem defaultUIItem;

    public Text totalText;

    public Reactive<int> total = new Reactive<int>();


    public override void AssocateData()
    {
        rxUIControl.Binding(dataSource, defaultUIItem);

        rxUIControl.Binding(total, totalText);

        dataSource.CombinePropertyTo(d=>d.Value, ()=> { total.SetValue(dataSource.Sum(d => d.Value.GetValue())); });
    }

    void Start()
    {
        defaultUIItem.gameObject.SetActive(false);
    }

    public void OnAddButton()
    {
        dataSource.Add(new Data());
    }

    public void OnRemoveButton()
    {
        dataSource.Remove(dataSource.Last());
    }
}

public class Data
{
    public Reactive<int> Value;

    public Data(int init)
    {
        Value = new Reactive<int>(init);
    }

    public Data()
    {
        Value = new Reactive<int>();
    }
}