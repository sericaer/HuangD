using HuangD.Interfaces;
using Math.TileMap;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class ProvinceNames : MonoBehaviour
{
    public ProvinceMapUI provMapUITemplate;

    public Grid mapGrid;

    private List<ProvinceMapUI> list { get; } = new List<ProvinceMapUI>();

    //internal void SetProvinces(Dictionary<IProvince, Block> province2Block)
    //{
    //    list = new List<ProvinceMapUI>();

    //    foreach (var pair in province2Block)
    //    {
    //        var pos = HuangD.Maps.Utilty.GetCenterPos(pair.Value.elements);

    //        var provMapUI = Instantiate<ProvinceMapUI>(provMapUITemplate, this.transform);
    //        list.Add(provMapUI);

    //        provMapUI.gmData = pair.Key;
    //        provMapUI.cellPos = pos;

    //        list.Add(provMapUI);
    //    }

    //    provMapUITemplate.gameObject.SetActive(false);

    //    UpdateNamePosition();
    //}

    public void UpdateNamePosition()
    {
        foreach (var elem in list)
        {
            var worldPos = mapGrid.CellToWorld(new Vector3Int(elem.cellPos.x, elem.cellPos.y));

            elem.transform.position = worldPos;
        }

    }

    // Start is called before the first frame update
    void Start()
    {
        provMapUITemplate.gameObject.SetActive(false);

        UpdateNamePosition();
    }

    // Update is called once per frame
    void Update()
    {

    }

    internal void AddProvince(IProvince province)
    {
        var pos = HuangD.Maps.Utilty.GetCenterPos(province.block.elements);

        var provMapUI = Instantiate<ProvinceMapUI>(provMapUITemplate, this.transform);
        provMapUI.gmData = province;
        provMapUI.cellPos = pos;


        provMapUI.gameObject.SetActive(true);
        list.Add(provMapUI);
    }

    internal void Clear()
    {
        if(list == null)
        {
            return;
        }

        foreach(var elem in list)
        {
            Destroy(elem.gameObject);
        }

        list.Clear();
    }
}
