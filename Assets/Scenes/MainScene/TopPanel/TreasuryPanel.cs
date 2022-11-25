using HuangD.Interfaces;
using UnityEngine.UI;

public class TreasuryPanel : UIBehaviour<ITreasury>
{
    public Text stock;
    public IncFlag incFlag;

    protected override void AssocDataSource()
    {
        Bind(taxMgr => System.Math.Round(taxMgr.stock,1), stock);
        Bind(taxMgr => taxMgr.surplus > 0, incFlag);
    }
}