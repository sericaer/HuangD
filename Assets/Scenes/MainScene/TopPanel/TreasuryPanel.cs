using HuangD.Interfaces;
using UnityEngine.Events;
using UnityEngine.UI;

public class TreasuryPanel : UIBehaviour<ITreasury>
{
    public Text stock;
    public IncFlag incFlag;

    public UnityEvent<ITreasury> showTreasuryDetail;

    public void OnClick()
    {
        showTreasuryDetail.Invoke(dataSource);
    }

    protected override void AssocDataSource()
    {
        Bind(taxMgr => $"{System.Math.Round(taxMgr.stock / 1000.0, 1)}K", stock);
        Bind(taxMgr => taxMgr.surplus > 0, incFlag);
    }
}