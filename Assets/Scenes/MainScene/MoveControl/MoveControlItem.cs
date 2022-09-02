using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class MoveControlItem : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public static bool isOn;

    // Start is called before the first frame update
    void Start()
    {
        isOn = false;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        isOn = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        isOn = false;
    }
}
