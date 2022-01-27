using UnityEngine;

using System.Collections.Generic;
using System.Collections;

using DataStructures;
using GraphNamespace;

namespace VisualPathFinderNamespace
{
    public class VisualPathFinder : MonoBehaviour
    {
        internal int Speed = 1;
        private object visualHeuristic = null;
        private bool isSearching = false;
        public bool IsSearching { get { return isSearching; } }

        public void SetHeuristic<H, N>(H heuristic)
            where H : IHeuristicInterface<N>
            where N : IVisualNodeInterface
        {
            visualHeuristic = heuristic;
        }

        public void AStar<N>(Graph<N> graph, N start, HashSet<N> interestPoints)
            where N : IVisualNodeInterface
        {
            if (visualHeuristic == null)
            {
                throw new System.ArgumentNullException("Heuristic has not been set!");
            }

            StartCoroutine(AStarCoroutine(graph, start, interestPoints, false));
        }

        private IEnumerator AStarCoroutine<N>
            (Graph<N> graph, N start, HashSet<N> interestPoints, bool isDijkstra)
                where N : IVisualNodeInterface
        {
            isSearching = true;
            var i = 0;
            var heuristic = (isDijkstra) ? new DijkstraHeuristic<N>() : (IHeuristicInterface<N>)visualHeuristic;
            var ips = new HashSet<N>(interestPoints);

            while (ips.Count != 0)
            {
                var done = true;

                var distanceMap = new Dictionary<N, float>();
                distanceMap.Add(start, 0);

                var closest = GetClosestIP(start, ips);
                closest.MarkClosest();
                yield return null;

                var bestGuessMap = new Dictionary<N, float>();
                bestGuessMap.Add(start, heuristic.EstimateDistance(start, closest));

                var cmp = new A_StarComparer<N>(bestGuessMap);

                var candidateQueue = new PriorityQueue<N>(cmp);
                candidateQueue.Offer(start);

                var previousMap = new Dictionary<N, N>();

                while (candidateQueue.Count > 0)
                {
                    var current = candidateQueue.Poll();

                    if (ips.Contains(current))
                    {
                        ips.Remove(current);
                        yield return MakePath(current, previousMap);
                        start = current;
                        done = false;
                        break;
                    }

                    current.MarkVisited();
                    if (i % Speed == 0)
                    {
                        yield return null;
                    }
                    i++;

                    var neighbours = graph.GetNeighbours(current);
                    if (neighbours == null)
                    {
                        continue;
                    }

                    foreach (var n in neighbours)
                    {
                        distanceMap.TryAdd(n, float.PositiveInfinity);
                        bestGuessMap.TryAdd(n, float.PositiveInfinity);

                        var tentativeDistance = distanceMap[current] + graph.GetEdgeWeight(current, n);

                        if (tentativeDistance.CompareTo(distanceMap[n]) < 0)
                        {
                            distanceMap[n] = tentativeDistance;
                            bestGuessMap[n] = tentativeDistance + heuristic.EstimateDistance(n, closest);

                            if (previousMap.TryAdd(n, current) == false)
                            {
                                previousMap[n] = current;
                            }

                            if (candidateQueue.Contains(n) == false)
                            {
                                n.MarkCandidate();
                                if (i % Speed == 0)
                                {
                                    yield return null;
                                }
                                i++;

                                candidateQueue.Offer(n);
                            }
                        }
                    }
                }

                if (done)
                {
                    break;
                }
            }

            isSearching = false;
        }

        private N GetClosestIP<N>(N start, HashSet<N> ips)
            where N : IVisualNodeInterface
        {
            IVisualNodeInterface closest = null;
            foreach (var n in ips)
            {
                if (closest == null)
                {
                    closest = n;
                    continue;
                }

                if (((IHeuristicInterface<N>)visualHeuristic).EstimateDistance(start, n).CompareTo(
                    ((IHeuristicInterface<N>)visualHeuristic).EstimateDistance(start, (N)closest)) < 0)
                {
                    closest = n;
                }
            }

            return (N)closest;
        }

        private IEnumerator MakePath<N>(N current, Dictionary<N, N> previousMap)
            where N : IVisualNodeInterface
        {
            var i = 0;

            while (true)
            {
                current.MarkPath();

                if (i % Mathf.Max(1, Speed / 2) == 0)
                {
                    yield return null;
                }
                i++;

                if (previousMap.TryGetValue(current, out current) == false)
                {
                    break;
                }
            }
        }
        public void Dijkstra<N>(Graph<N> graph, N start, HashSet<N> interestPoints)
            where N : IVisualNodeInterface
        {
            if (visualHeuristic == null)
            {
                throw new System.ArgumentNullException("Heuristic has not been set!");
            }

            StartCoroutine(AStarCoroutine(graph, start, interestPoints, true));
        }

        private class A_StarComparer<N> : IComparer<N> where N : IVisualNodeInterface
        {
            private readonly Dictionary<N, float> bestGuessMap;

            public A_StarComparer(Dictionary<N, float> bestGuessMap)
            {
                this.bestGuessMap = bestGuessMap;
            }

            public int Compare(N a, N b)
            {
                return bestGuessMap[a].CompareTo(bestGuessMap[b]);
            }
        }

        private class DijkstraHeuristic<N> : IHeuristicInterface<N> where N : IVisualNodeInterface
        {
            public float EstimateDistance(N node, N end)
            {
                return 0;
            }
        }

        public void Stop()
        {
            StopAllCoroutines();
            isSearching = false;
        }
    }

    public interface IHeuristicInterface<N> where N : IVisualNodeInterface
    {
        float EstimateDistance(N node, N end);
    }
}