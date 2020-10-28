using System;
using System.Collections.Generic;

class PriorityQueue<T> where T : IComparable<T> {

    List<T> buffer;
    public T Top => this.buffer[0];
    public int Count => this.buffer.Count;

    public PriorityQueue() {
        this.buffer = new List<T>();
    }

    public PriorityQueue(int capacity) {
        this.buffer = new List<T>(capacity);
    }

    private static void PushHeap(List<T> array, T elem) {
        int n = array.Count;
        array.Add(elem);

        while (n != 0) {
            int i = (n - 1) / 2;
            if (array[n].CompareTo(array[i]) > 0) {
                T tmp = array[n]; array[n] = array[i]; array[i] = tmp;
            }
            n = i;
        }
    }

    private static void PopHeap(List<T> array) {
        int n = array.Count - 1;
        array[0] = array[n];
        array.RemoveAt(n);

        for (int i = 0, j; (j = 2 * i + 1) < n;) {
            if ((j != n - 1) && (array[j].CompareTo(array[j + 1]) < 0)) j++;
            if (array[i].CompareTo(array[j]) < 0) {
                T tmp = array[j]; array[j] = array[i]; array[i] = tmp;
            }
            i = j;
        }
    }

    public void Push(T elem) {
        PushHeap(this.buffer, elem);
    }

    public void Pop() {
        PopHeap(this.buffer);
    }

}