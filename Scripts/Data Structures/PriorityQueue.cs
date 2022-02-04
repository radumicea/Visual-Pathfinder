using System;
using System.Collections.Generic;

namespace DataStructures
{
    public class PriorityQueue<N>
    {
        private const int DefaultCapacity = 16;
        private N[] heap;
        private int count;
        private readonly IComparer<N> cmp;

        public int Count { get { return count; } }
        public N First
        {
            get
            {
                if (count == 0)
                {
                    throw new InvalidOperationException("The priorityQueue is empty!");
                }

                return heap[0];
            }
        }

        public PriorityQueue() : this(Comparer<N>.Default) { }

        public PriorityQueue(IComparer<N> comparer)
        {
            if (comparer == null)
            {
                throw new ArgumentNullException(nameof(comparer));
            }

            heap = new N[DefaultCapacity];
            cmp = comparer;
        }

        public PriorityQueue(int capacity) : this(capacity, Comparer<N>.Default) { }

        public PriorityQueue(int capacity, IComparer<N> comparer)
        {
            if (comparer == null)
            {
                throw new ArgumentNullException(nameof(comparer));
            }

            heap = new N[GetCapacity(capacity)];
            cmp = comparer;
        }

        public PriorityQueue(ICollection<N> collection) : this(collection, Comparer<N>.Default) { }

        public PriorityQueue(ICollection<N> collection, IComparer<N> comparer)
        {
            if (collection == null)
            {
                throw new ArgumentNullException(nameof(collection));
            }
            if (comparer == null)
            {
                throw new ArgumentNullException(nameof(comparer));
            }

            heap = new N[GetCapacity(collection.Count)];
            collection.CopyTo(heap, 0);
            count = collection.Count;
            cmp = comparer;
            Heapify();
        }

        private PriorityQueue(N[] heap, int count, IComparer<N> cmp)
        {
            this.heap = heap;
            this.count = count;
            this.cmp = cmp;
        }

        private static int GetCapacity(int capacity)
        {
            int n = DefaultCapacity;

            if (capacity >= n)
            {
                n = GetNextPow2(capacity);

                if (n < DefaultCapacity)
                {
                    n = DefaultCapacity;
                }
            }

            return n;
        }

        private static int GetNextPow2(int n)
        {
            n |= (n >> 1);
            n |= (n >> 2);
            n |= (n >> 4);
            n |= (n >> 8);
            n |= (n >> 16);

            return n + 1;
        }

        public PriorityQueue<N> Merge(PriorityQueue<N> priorityQueue)
        {
            if (priorityQueue == null)
            {
                throw new ArgumentNullException(nameof(priorityQueue));
            }

            var heap = new N[GetNextPow2(this.count + priorityQueue.count)];
            Array.Copy(this.heap, heap, this.count);
            Array.Copy(priorityQueue.heap, 0, heap, this.count, priorityQueue.count);
            var pq = new PriorityQueue<N>(heap, this.count + priorityQueue.count, this.cmp);
            pq.Heapify();

            return pq;
        }

        private void Heapify()
        {
            for (int parent = count / 2 - 1; parent >= 0; parent--)
            {
                SiftDown(parent);
            }
        }

        public N Poll()
        {
            if (count == 0)
            {
                throw new InvalidOperationException("Priority Queue is empty!");
            }

            N item = heap[0];

            Remove(0);

            return item;
        }

        private void Remove(int index)
        {
            if (count == 0)
            {
                throw new InvalidOperationException("Priority Queue is empty!");
            }

            if (index < 0)
            {
                throw new IndexOutOfRangeException("index" + index + " < 0");
            }

            if (index >= count)
            {
                throw new IndexOutOfRangeException("Index: " + index + ", Size: " + count);
            }

            heap[index] = heap[count - 1];
            count--;
            SiftDown(index);
        }

        public void ReplaceFirst(N node)
        {
            if (count == 0)
            {
                throw new InvalidOperationException("The priorityQueue is empty!");
            }

            heap[0] = node;
            SiftDown(0);
        }

        private void SiftDown(int parent)
        {
            if (parent < 0)
            {
                throw new IndexOutOfRangeException("index: " + parent + " < 0");
            }

            if (parent > count)
            {
                throw new IndexOutOfRangeException("Index: " + parent + ", Size: " + count);
            }

            var l = parent * 2 + 1;
            var r = parent * 2 + 2;

            while (l < count)
            {
                var swapAt = parent;

                if (cmp.Compare(heap[swapAt], heap[l]) > 0)
                {
                    swapAt = l;
                }

                if (r < count && cmp.Compare(heap[swapAt], heap[r]) > 0)
                {
                    swapAt = r;
                }

                if (swapAt == parent)
                {
                    return;
                }

                (heap[swapAt], heap[parent]) = (heap[parent], heap[swapAt]);

                parent = swapAt;
                l = parent * 2 + 1;
                r = parent * 2 + 2;
            }
        }

        public void Offer(N item)
        {
            if (count == heap.Length)
            {
                Array.Resize(ref heap, count << 1);
            }

            heap[count] = item;

            SiftUp(count);
            count++;
        }

        private void SiftUp(int child)
        {
            if (child < 0)
            {
                throw new IndexOutOfRangeException("index: " + child + " < 0");
            }

            if (child > count)
            {
                throw new IndexOutOfRangeException("Index: " + child + ", Size: " + count);
            }

            var parent = (child + 1) / 2 - 1;

            while (child != 0 && (cmp.Compare(heap[child], heap[parent]) < 0))
            {
                (heap[child], heap[parent]) = (heap[parent], heap[child]);

                child = parent;
                parent = (child + 1) / 2 - 1;
            }
        }

        public bool Contains(N item)
        {
            return (Array.IndexOf(heap, item) != -1);
        }
    }
}