using HuangD.Interfaces;
using UnityEngine.UI;

public class TaxItem : UIBehaviour<ITreasury.IIncomeItem>
{
    public Text label;
    public Text value;
    public ToggleGroupEx levels;

    protected override void AssocDataSource()
    {
        Bind(income => income.type, label);
        Bind(income => income.GetValue(), value);

        BindTwoWay(income => income.level, levels);
    }
}
