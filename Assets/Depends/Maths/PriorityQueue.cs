using System.Collections.Generic;

namespace Math.TileMap
{
    class PriorityQueue<T>
    {
        struct Item
        {
            public T data;
            public float pri;

            public Item(T data, float pri)
            {
                this.data = data;
                this.pri = pri;
            }
        }

        class Comparer : IComparer<Item>
        {
            public int Compare(Item x, Item y)
            {
                return x.pri.CompareTo(y.pri);
            }
        }

        List<Item> list = new List<Item>();

        Comparer comparer = new Comparer();

        public void Enqueue(T data, float pri)
        {
            list.Add(new Item(data, pri));

            list.Sort(comparer);
        }

        public T Dequeue()
        {
            var first = list[0];
            list.RemoveAt(0);

            return first.data;
        }

        public int Count => list.Count;
    }
}
