using ModelShark;
using System;
using UnityEngine;

[RequireComponent(typeof(TooltipTrigger))]
public class DynamicToolTip : MonoBehaviour
{
    public Func<string> GenerateBody { get; set; }
    public Func<string> GenerateTitle { get; set; }

    private void Start()
    {
        var trigger = GetComponent<TooltipTrigger>();
        trigger.actionBeforPopup = () =>
        {
            if(GenerateBody != null)
            {
                trigger.SetText("BodyText", GenerateBody());
            }
            if (GenerateTitle != null)
            {
                trigger.SetText("TitleText", GenerateTitle());
            }
        };
    }
}
