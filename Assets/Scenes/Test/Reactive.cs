using System;
using System.Reactive.Linq;
using System.Reactive.Subjects;

public class Reactive<T> : IObservable<T>, IDisposable
{
    private T _data;
    private Subject<T> subject;

    public Reactive()
    {
        subject = new Subject<T>();

        subject.Subscribe(data => _data = data);
    }

    public Reactive(T Value) : this()
    {
        subject.OnNext(Value);
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
