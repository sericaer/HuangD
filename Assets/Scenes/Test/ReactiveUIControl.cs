using System;
using System.Collections.Generic;
using System.Reactive.Linq;
using UnityEngine.UI;
using System.Linq;
using System.Linq.Expressions;

public class ReactiveUIControl : IDisposable
{
    private List<BindItem> bindItems = new List<BindItem>();

    public void Dispose()
    {
        Clear();
    }

    public void Clear()
    {
        foreach (var item in bindItems)
        {
            item.Dispose();
        }

        bindItems.Clear();
    }

    internal void Binding<TFrom, TProperty>(
        ReactiveList<TFrom> reactivelist, 
        Func<TFrom, IObservable<TProperty>> fromProperty, 
        Text text, 
        Func<object> transform)
    {
        foreach (var item in reactivelist)
        {
            fromProperty(item);
        }
    }

    public BindItem Binding<T>(Reactive<T> reactiveData, Text text)
    {
        text.text = reactiveData.GetValue().ToString();

        var bindItem = new BindItem(reactiveData.Subscribe(data => text.text = data.ToString()));
        bindItems.Add(bindItem);

        return bindItem;
    }

    public BindItem Binding<T>(ReactiveList<T> reactivelist, RxBehaviour<T> rxBehaviour)
    {
        foreach(var item in reactivelist)
        {
            var newRxBehaviour = UnityEngine.Object.Instantiate(rxBehaviour, rxBehaviour.gameObject.transform.parent);
            newRxBehaviour.SetDataSource(item);
            newRxBehaviour.gameObject.SetActive(true);
        }

        var bindItem = new BindItem(reactivelist.OnChanged.Subscribe(changeSet => 
        {
            switch(changeSet.changedType)
            {
                case ReactiveList<T>.ChangeSet.ChangedType.ADD:
                    {
                        var newRxBehaviour = UnityEngine.Object.Instantiate(rxBehaviour, rxBehaviour.gameObject.transform.parent);
                        newRxBehaviour.SetDataSource(changeSet.data);
                        newRxBehaviour.gameObject.SetActive(true);

                    }
                    break;
                case ReactiveList<T>.ChangeSet.ChangedType.Remove:
                    {
                        var needDestory = rxBehaviour.gameObject.transform.parent.GetComponentsInChildren<RxBehaviour<T>>()
                            .SingleOrDefault(x =>
                            {
                                if (x.dataSource is T)
                                {
                                    return EqualityComparer<T>.Default.Equals(changeSet.data, (T)x.dataSource);
                                }

                                return false;
                            });
                        if(needDestory != null)
                        {
                            UnityEngine.Object.Destroy(needDestory.gameObject);
                        }
                    }
                    break;
                default:
                    throw new Exception();
            }
        }));

        bindItems.Add(bindItem);

        return bindItem;
    }

    public class BindItem : IDisposable
    {
        public void Dispose()
        {
            if(isDisposed)
            {
                return;
            }

            isDisposed = true;
            SubscribeRslt.Dispose();
        }

        public BindItem(IDisposable disposable)
        {
            this.SubscribeRslt = disposable;
        }

        private IDisposable SubscribeRslt;
        private bool isDisposed = false;
    }
}
