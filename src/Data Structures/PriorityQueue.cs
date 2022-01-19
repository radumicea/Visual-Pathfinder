using System.Collections.Generic;

namespace DataStructures
{
    public class PriorityQueue<N>
    {
        private readonly List<N> nodes;
        private int count;
        private readonly IComparer<N> cmp;

        public int Count { get { return count; } }

        public PriorityQueue(IComparer<N> comparer)
        {
            nodes = new List<N>();
            count = 0;
            cmp = comparer;
        }

        public PriorityQueue(int count, IComparer<N> comparer)
        {
            nodes = new List<N>(count);
            this.count = 0;
            cmp = comparer;
        }

        public PriorityQueue(List<N> elements, IComparer<N> comparer)
        {
            nodes = elements;
            count = nodes.Count;
            cmp = comparer;
            Heapify();
        }

        public PriorityQueue<N> Merge(PriorityQueue<N> priorityQueue)
        {
            var nodes = new List<N>(this.nodes);
            nodes.AddRange(priorityQueue.nodes);

            return new PriorityQueue<N>(nodes, cmp);
        }

        private void Heapify()
        {
            for (var parent = count / 2 - 1; parent >= 0; parent--)
            {
                SiftDown(parent);
            }
        }

        public N Poll()
        {
            if (count == 0)
            {
                throw new System.IndexOutOfRangeException("The priorityQueue is empty!");
            }

            N retNode = nodes[0];

            Remove(0);

            return retNode;
        }

        private void Remove(int index)
        {
            if (count == 0)
            {
                throw new System.IndexOutOfRangeException("The priorityQueue is empty!");
            }

            if (index < 0)
            {
                throw new System.IndexOutOfRangeException("index" + index + " < 0");
            }

            if (index >= count)
            {
                throw new System.IndexOutOfRangeException("Index: " + index + ", Size: " + count);
            }

            nodes[index] = nodes[count - 1];
            count--;
            SiftDown(index);
        }

        public void ReplaceFirst(N node)
        {
            if (count == 0)
            {
                throw new System.IndexOutOfRangeException("The priorityQueue is empty!");
            }

            nodes[0] = node;
            SiftDown(0);
        }

        private void SiftDown(int parent)
        {
            if (parent < 0)
            {
                throw new System.IndexOutOfRangeException("index: " + parent + " < 0");
            }

            if (parent > count)
            {
                throw new System.IndexOutOfRangeException("Index: " + parent + ", Size: " + count);
            }

            var l = parent * 2 + 1;
            var r = parent * 2 + 2;

            while (l < count)
            {
                var swapAt = parent;

                if (cmp.Compare(nodes[swapAt], nodes[l]) > 0)
                {
                    swapAt = l;
                }

                if (r < count && cmp.Compare(nodes[swapAt], nodes[r]) > 0)
                {
                    swapAt = r;
                }

                if (swapAt == parent)
                {
                    return;
                }

                (nodes[swapAt], nodes[parent]) = (nodes[parent], nodes[swapAt]);

                parent = swapAt;
                l = parent * 2 + 1;
                r = parent * 2 + 2;
            }
        }

        public N Peek()
        {
            if (count == 0)
            {
                throw new System.IndexOutOfRangeException("The priorityQueue is empty!");
            }

            return nodes[0];
        }

        public void Offer(N element)
        {
            if (count == nodes.Count)
            {
                nodes.Add(element);
            }

            else
            {
                nodes[count] = element;
            }

            SiftUp(count);
            count++;
        }

        private void SiftUp(int child)
        {
            if (child < 0 || child > count)
            {
                throw new System.IndexOutOfRangeException();
            }

            var parent = (child + 1) / 2 - 1;

            while (child != 0 && (cmp.Compare(nodes[child], nodes[parent]) < 0))
            {
                (nodes[child], nodes[parent]) = (nodes[parent], nodes[child]);

                child = parent;
                parent = (child + 1) / 2 - 1;
            }
        }

        public bool Contains(N element)
        {
            return nodes.Contains(element);
        }
    }
}