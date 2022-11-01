using System.Linq;
using UnityEngine.UI;

public class ReactiveListTest : RxListBehaviour<Data>
{
    public ReactiveListItem defaultUIItem;
    
    public override void AssocateData()
    {
        rxUIControl.Binding(dataSource, defaultUIItem);
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
        dataSource.Remove(dataSource.GetDatas().Last());
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