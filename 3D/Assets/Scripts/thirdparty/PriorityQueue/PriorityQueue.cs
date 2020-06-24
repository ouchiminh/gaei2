/**
 * [PriorityQueue.cs]
 * Copyright (c) 2020 ouchiminh
 * This software is released under the MIT License.
 */
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace PriorityQueue
{
    public class PriorityQueue<T>
    {
        public delegate int Comparer(T a, T b);
        public PriorityQueue(Comparer c) { comparer_ = c; container_ = new List<T>(); }
        public PriorityQueue() : this(Comparer<T>.Default.Compare){}
        public void Enqueue(T elem) {
            var n = container_.Count;
            container_.Add(elem);
            while (n > 0 && comparer_(container_[(n-1)/2], container_[n])>0)
            {
                T buf = container_[n];
                container_[n] = container_[(n - 1) / 2];
                container_[(n - 1) / 2] = buf;
                n = (n - 1) / 2;
            }
        }
        public void Enqueue(IEnumerable<T> enumerable) { foreach (var x in enumerable) Enqueue(x); }
        public T Dequeue() {
            var x = Top();
            Pop();
            return x;
        }
        public void Pop() {
            container_[0] = container_[container_.Count - 1];
            container_.RemoveAt(container_.Count - 1);
            var n = 0;
            while (2*n+1 < container_.Count)
            {
                (T, int) lesschild = (2 * n) + 2 < container_.Count && comparer_(container_[2 * n + 2], container_[2 * n + 1]) < 0
                    ? (container_[2 * n + 2], 2*n+2) : (container_[2 * n + 1], 2*n+1);
                if (comparer_(container_[n], lesschild.Item1) > 0)
                {
                    container_[lesschild.Item2] = container_[n];
                    container_[n] = lesschild.Item1;
                    n = lesschild.Item2;
                }
                else break;
            }
        }
        public T Top() { return container_[0]; }
        public bool Empty { get => Count == 0; }
        public int Count { get => container_.Count; }
        private Comparer comparer_;
        private List<T> container_;
    }
}
