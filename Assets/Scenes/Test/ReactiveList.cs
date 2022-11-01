using System;
using System.Collections.Generic;
using System.Reactive.Linq;
using System.Reactive.Subjects;

public class ReactiveList<T>
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
}