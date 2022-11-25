using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class MapToggles : MonoBehaviour
{
    [Serializable]
    public struct ToggleItem
    {
        public Toggle toggle;
        public GameObject map;
    }

    public ToggleItem[] toggleItems;

    // Start is called before the first frame update
    void Start()
    {

        foreach (var item in toggleItems)
        {
            item.map.SetActive(item.toggle.isOn);

            item.toggle.onValueChanged.AddListener((isOn) =>
            {
                item.map.SetActive(isOn);
            });
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
