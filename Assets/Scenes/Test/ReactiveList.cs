using System;
using System.Collections;
using System.Collections.Generic;
using System.Reactive.Linq;
using System.Reactive.Subjects;

public class ReactiveList<T> : IEnumerable<T>
{
    public class ChangeSet
    {
        public enum ChangedType
        {
            ADD,
            Remove,
        }

        public readonly ChangedType changedType;
        public readonly T data;

        public ChangeSet(ChangedType changedType, T data)
        {
            this.changedType = changedType;
            this.data = data;
        }
    }

    public IObservable<ChangeSet> OnChanged => subject;

    private List<T> list = new List<T>();
    private Subject<ChangeSet> subject = new Subject<ChangeSet>();

    private Dictionary<T, IDisposable> dictCombine = new Dictionary<T, IDisposable>();
    public ReactiveList()
    {
        OnChanged.Subscribe(changeSet =>
        {
            switch (changeSet.changedType)
            {
                case ChangeSet.ChangedType.ADD:
                    {
                        list.Add(changeSet.data);
                    }
                    break;
                case ChangeSet.ChangedType.Remove:
                    {
                        list.Remove(changeSet.data);
                    }
                    break;
                default:
                    throw new Exception();
            }
        });
    }

    public ReactiveList(IEnumerable<T> init) : this()
    {
        foreach(var item in init)
        {
            Add(item);
        }
    }
    public IEnumerable<T> GetDatas()
    {
        return list;
    }

    public void Add(T item)
    {
        subject.OnNext(new ChangeSet(ChangeSet.ChangedType.ADD, item));
    }

    public void Remove(T item)
    {
        subject.OnNext(new ChangeSet(ChangeSet.ChangedType.Remove, item));
    }

    public IEnumerator<T> GetEnumerator()
    {
        return ((IEnumerable<T>)list).GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return ((IEnumerable)list).GetEnumerator();
    }

    public class CombineDispose : IDisposable
    {
        public IDisposable totoalDispose;
        public HashSet<IDisposable> subDisposes = new HashSet<IDisposable>();

        public void Dispose()
        {
            totoalDispose.Dispose();

            foreach(var elem in subDisposes)
            {
                elem.Dispose();
            }
        }
    }

    public IDisposable CombinePropertyTo<TProperty>(Func<T, IObservable<TProperty>> fromProperty, Action notifiedMethod)
    {
        var combineDispose = new CombineDispose();

        foreach(var item in this)
        {
            var dispose = fromProperty(item).Subscribe(_ => notifiedMethod());

            dictCombine.Add(item, dispose);
            combineDispose.subDisposes.Add(dispose);
        }

        combineDispose.totoalDispose = OnChanged.Subscribe(changeSet =>
        {
            switch (changeSet.changedType)
            {
                case ReactiveList<Data>.ChangeSet.ChangedType.ADD:
                    {
                        var dispose = fromProperty(changeSet.data).Subscribe(_ => notifiedMethod());

                        dictCombine.Add(changeSet.data, dispose);
                        combineDispose.subDisposes.Add(dispose);
                    }
                    break;
                case ReactiveList<Data>.ChangeSet.ChangedType.Remove:
                    if (dictCombine.ContainsKey(changeSet.data))
                    {
                        var dispose = dictCombine[changeSet.data];
                        dispose.Dispose();

                        dictCombine.Remove(changeSet.data);
                        combineDispose.subDisposes.Remove(dispose);
                    }
                    break;
                default:
                    throw new Exception();
            }
        });

        return combineDispose;
    }
}