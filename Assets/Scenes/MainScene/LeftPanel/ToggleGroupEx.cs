using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class ToggleGroupEx : MonoBehaviour
{
    public Toggle defaultItem;

    private Dictionary<Toggle, Enum> toggle2Enum = new Dictionary<Toggle, Enum>();

    private void Start()
    {
        defaultItem.gameObject.SetActive(false);
    }

    internal Toggle Add<TValue>(TValue item)
        where TValue : Enum
    {
        var newInstance = Instantiate<Toggle>(defaultItem, defaultItem.transform.parent);
        newInstance.name = item.ToString();
        newInstance.GetComponentInChildren<Text>().text = item.ToString();
        newInstance.gameObject.SetActive(true);

        toggle2Enum.Add(newInstance, item);

        return newInstance;
    }

    internal TValue GetEnum<TValue>(Toggle toggle)
        where TValue : Enum
    {
        return (TValue)toggle2Enum[toggle];
    }

    internal void Clear()
    {
        foreach(var toggle in toggle2Enum.Keys.Where(t=> t != defaultItem))
        {
            Destroy(toggle);
        }

        toggle2Enum.Clear();
    }
}