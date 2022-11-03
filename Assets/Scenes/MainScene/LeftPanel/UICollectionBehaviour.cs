using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UICollectionBehaviour<TItem, TUIItem> : MonoBehaviour  
    where TItem : class
    where TUIItem : UIBehaviour<TItem>
{
    public TUIItem defaultUIItem;
    protected Transform uiContainer => defaultUIItem.gameObject.transform.parent;

    private object dataSource;
    private Func<object, IEnumerable<TItem>> func;


    public void SetDataSource(object dataSource, Func<object, IEnumerable<TItem>> func) 
    {
        this.dataSource = dataSource;
        this.func = func;

        AssocateData();
    }

    protected virtual void AssocateData()
    {

    }

    protected virtual void Start()
    {
        defaultUIItem.gameObject.SetActive(false);
    }

    protected virtual void FixedUpdate()
    {
        var datas = func(dataSource);
        var uiItems = uiContainer.GetComponentsInChildren<UIBehaviour<TItem>>();

        foreach (var data in datas)
        {
            if(uiItems.All(x=>x.dataSource != data))
            {
                var uiItem = Instantiate(defaultUIItem, uiContainer) as UIBehaviour<TItem>;
                uiItem.dataSource = data;
                uiItem.gameObject.SetActive(true);
            }
        }

        foreach(var ui in uiItems.Where(u=>u!= defaultUIItem))
        {
            if(datas.All(x=>x != ui.dataSource))
            {
                Destroy(ui.gameObject);
            }
        }
    }
}
