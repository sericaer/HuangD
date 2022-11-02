using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class UIBehaviour<T> : MonoBehaviour
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

            dictText = new Dictionary<Text, Func<T, object>>();

            _dataSource = value;

            AssocDataSource();
        }
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

    protected void Bind<TItem>(Func<T, IEnumerable<TItem>> func, UICollectionBehaviour<TItem> uiContainer)
        where TItem : class
    {
        Func<object, IEnumerable<TItem>> adpt = (object obj) => func((T)obj);
        uiContainer.SetDataSource(dataSource, adpt);
    }

    protected abstract void AssocDataSource();
}