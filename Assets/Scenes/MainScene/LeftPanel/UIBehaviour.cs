using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class UIBehaviour<T> : MonoBehaviour
    where T:class
{
    private T _dataSource;

    private Dictionary<Text, Func<T, object>> dictText;

    public T dataSource
    {
        get
        {
            return _dataSource;
        }
        set
        {
            if(_dataSource == value)
            {
                return;
            }

            dictText = new Dictionary<Text, Func<T, object>>();

            _dataSource = value;

            AssocDataSource();
        }
    }

    protected virtual void Awake()
    {

    }

    protected virtual void Start()
    {

    }


    protected virtual void FixedUpdate()
    {
        foreach(var pair in dictText)
        {
            pair.Key.text = pair.Value(dataSource).ToString();
        }
    }

    protected virtual void OnDestroy()
    {

    }

    protected void Bind(Func<T, object> func, Text text)
    {
        dictText.Add(text, func);
    }

    protected void Bind<TItem, TUIItem>(Func<T, IEnumerable<TItem>> func, UICollectionBehaviour<TItem, TUIItem> uiContainer)
        where TItem : class
        where TUIItem : UIBehaviour<TItem>
    {
        Func<object, IEnumerable<TItem>> adpt = (object obj) => func((T)obj);
        uiContainer.SetDataSource(dataSource, adpt);
    }

    protected abstract void AssocDataSource();
}