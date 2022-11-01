using System;
using UnityEngine;

public abstract class RxBehaviour<T> : MonoBehaviour
{
    protected ReactiveUIControl rxUIControl = new ReactiveUIControl();
    public T dataSource { get; private set; }

    public void SetDataSource(T dataSource)
    {
        rxUIControl.Clear();

        this.dataSource = dataSource;

        AssocateData();
    }

    public abstract void AssocateData();

    private void OnDestroy()
    {
        rxUIControl.Dispose();
    }
}
