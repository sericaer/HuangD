using UnityEngine.UI;

public class ReactiveListItem : RxBehaviour<Data>
{
    public Text data;

    public override void AssocateData()
    {
        rxUIControl.Binding(dataSource.Value, data);
    }
}