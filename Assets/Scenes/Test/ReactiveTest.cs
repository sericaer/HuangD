using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using UnityEngine.UI;


public class ReactiveTest : RxBehaviour
{
    public Text test;

    Reactive<int> p = new Reactive<int>();

    int count = 0;
    // Start is called before the first frame update
    void Start()
    {
        rxUI.Binding<int>(p, test);
    }


    // Update is called once per frame
    void Update()
    {
        if (count % 100 == 0)
        {
            p.SetValue(count);
        }

        count++;
    }

    public void OnButton()
    {
        Destroy(this.gameObject);
    }
}

public class ReactiveUI : IDisposable
{
    private List<BindItem> bindItems = new List<BindItem>();

    public void Dispose()
    {
        foreach(var item in bindItems)
        {
            item.Dispose();
        }

        bindItems.Clear();
    }

    public BindItem Binding<T>(IObservable<T> reactiveData, Text text)
    {
        var bindItem = new BindItem(reactiveData.Subscribe(data => text.text = data.ToString()));
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


public class Reactive<T> : IObservable<T>, IDisposable
{
    private T _data;
    private Subject<T> subject;

    public Reactive()
    {
        subject = new Subject<T>();

        subject.Subscribe(data => _data = data);
    }

    public void SetValue(T Value)
    {
        subject.OnNext(Value);
    }

    public T GetValue()
    {
        return _data;
    }

    public IDisposable Subscribe(IObserver<T> observer)
    {
        return subject.Subscribe(observer);
    }

    public void Dispose()
    {
        subject.Dispose();
    }
}