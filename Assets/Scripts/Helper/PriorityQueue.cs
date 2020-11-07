using System;
using System.Collections.Generic;

namespace Helper
{
    internal class PriorityQueue<T> where T : IComparable<T>
    {
        private readonly List<T> _buffer;

        public PriorityQueue()
        {
            _buffer = new List<T>();
        }

        public PriorityQueue(int capacity)
        {
            _buffer = new List<T>(capacity);
        }

        public T Top => _buffer[0];
        public int Count => _buffer.Count;

        private static void PushHeap(IList<T> array, T elem)
        {
            var n = array.Count;
            array.Add(elem);

            while (n != 0)
            {
                var i = (n - 1) / 2;
                if (array[n].CompareTo(array[i]) > 0)
                {
                    var tmp = array[n];
                    array[n] = array[i];
                    array[i] = tmp;
                }

                n = i;
            }
        }

        private static void PopHeap(IList<T> array)
        {
            var n = array.Count - 1;
            array[0] = array[n];
            array.RemoveAt(n);

            for (int i = 0, j; (j = 2 * i + 1) < n;)
            {
                if (j != n - 1 && array[j].CompareTo(array[j + 1]) < 0) j++;
                if (array[i].CompareTo(array[j]) < 0)
                {
                    var tmp = array[j];
                    array[j] = array[i];
                    array[i] = tmp;
                }

                i = j;
            }
        }

        public void Push(T elem)
        {
            PushHeap(_buffer, elem);
        }

        public void Pop()
        {
            PopHeap(_buffer);
        }
    }
}