using HuangD.Interfaces;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class DatePanel : UIBehaviour<IDate>
{
    public Text year;
    public Text month;
    public Text day;

    protected override void AssocDataSource()
    {
        Bind(date => date.year, year);
        Bind(date => date.month, month);
        Bind(date => date.day, day);
    }
}