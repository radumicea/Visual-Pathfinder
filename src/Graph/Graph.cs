using System.Collections.Generic;

namespace GraphNamespace
{
    public class Graph<N> where N : INodeInterface
    {
        private readonly Dictionary<N, Dictionary<N, float>> adjacencyList;

        public Graph()
        {
            adjacencyList = new Dictionary<N, Dictionary<N, float>>();
        }

        public bool Contains(N node)
        {
            return (adjacencyList.ContainsKey(node) && node.IsAvailable);
        }

        public bool AreAdjacent(N x, N y)
        {
            return (this.Contains(x) && adjacencyList[x].ContainsKey(y));
        }

        public HashSet<N> GetNeighbours(N node)
        {
            if (this.Contains(node) == false)
            {
                return null;
            }

            var nodes = adjacencyList[node];
            var neighbours = new HashSet<N>();
            foreach (var n in nodes.Keys)
            {
                if (n.IsAvailable && node.CanGoTo(n))
                {
                    neighbours.Add(n);
                }
            }

            return neighbours;
        }

        public bool AddNode(N node)
        {
            if (this.Contains(node))
            {
                return false;
            }

            if (adjacencyList.ContainsKey(node))
            {
                node.IsAvailable = true;
            }

            else
            {
                adjacencyList.Add(node, new Dictionary<N, float>());
            }

            return true;
        }

        public bool RemoveNode(N node)
        {
            if (this.Contains(node) == false)
            {
                return false;
            }

            node.IsAvailable = false;

            return true;
        }

        public bool AddEdge(N source, N destination, float weight)
        {
            return (AddDirectedEdge(source, destination, weight) &&
                    AddDirectedEdge(destination, source, weight));
        }

        public bool AddDirectedEdge(N source, N destination, float weight)
        {
            if (this.Contains(source) == false ||
                this.Contains(destination) == false ||
                AreAdjacent(source, destination))
            {
                return false;
            }

            adjacencyList[source].Add(destination, weight);

            return SetEdgeWeight(source, destination, weight);
        }

        public bool SetEdgeWeight(N source, N destination, float weight)
        {
            if (AreAdjacent(source, destination) == false)
            {
                return false;
            }

            if (adjacencyList[source].TryAdd(destination, weight) == false)
            {
                adjacencyList[source][destination] = weight;
            }

            return true;
        }

        public float GetEdgeWeight(N source, N destination)
        {
            if (AreAdjacent(source, destination))
            {
                return adjacencyList[source][destination];
            }

            return -1;
        }

        public bool RemoveEdge(N source, N destination)
        {
            return (RemoveDirectedEdge(source, destination) &&
                    RemoveDirectedEdge(destination, source));
        }

        public bool RemoveDirectedEdge(N source, N destination)
        {
            if (this.Contains(source) == false ||
                this.Contains(destination) == false ||
                AreAdjacent(source, destination) == false)
            {
                return false;
            }

            adjacencyList[source].Remove(destination);

            return true;
        }
    }
}
