using UnityEngine;

using System;

using GraphNamespace;
using VisualPathFinderNamespace;

namespace VisualGridNamespace
{
    public class VisualGrid : MonoBehaviour
    {
        private static readonly float SQRT_2 = Mathf.Sqrt(2);
        internal static int ResizeFactor;

        internal static int Vertical;
        internal static int Horizontal;

        private static VisualTile[][] gridMap;
        private static Graph<VisualTile> gridGraph;

        [SerializeField] private VisualTile tilePrefab;

        public static VisualTile[][] GridMap { get { return gridMap; } }
        public static Graph<VisualTile> GridGraph { get { return gridGraph; } }

        private void Start()
        {
            GenerateGridMap();
        }

        private void GenerateGridMap()
        {
            gridGraph = new Graph<VisualTile>();

            if (Vertical <= 0 || Horizontal <= 0)
            {
                throw new ArgumentException("Grid sizes must be > 0");
            }

            gridMap = new VisualTile[Vertical][];

            for (var i = 0; i < Vertical; i++)
            {
                gridMap[i] = new VisualTile[Horizontal];

                for (var j = 0; j < Horizontal; j++)
                {
                    var tile = Instantiate(tilePrefab, new Vector3((float)j / ResizeFactor, (float)i / ResizeFactor), Quaternion.identity);
                    tile.name = "{" + (Vertical - i - 1) + ", " + j + "}";
                    tile.Init(i, j);
                    gridMap[i][j] = tile;
                    gridGraph.AddNode(tile);

                    if (i > 0)
                    {
                        gridGraph.AddEdge(tile, gridMap[i - 1][j], 1 / SQRT_2);
                    }

                    if (j > 0)
                    {
                        gridGraph.AddEdge(tile, gridMap[i][j - 1], 1 / SQRT_2);
                    }

                    if (i > 0 && j > 0)
                    {
                        gridGraph.AddEdge(tile, gridMap[i - 1][j - 1], 1);
                    }

                    if (i > 0 && j + 1 < Horizontal)
                    {
                        gridGraph.AddEdge(tile, gridMap[i - 1][j + 1], 1);
                    }
                }
            }
        }

        public static bool MarkUnavailable(int i, int j)
        {
            if (i >= Vertical || i < 0 ||
                j >= Horizontal || j < 0)
            {
                throw new IndexOutOfRangeException("Sizes must be > 0 and < gridMap sizes");
            }

            return gridGraph.RemoveNode(gridMap[i][j]);
        }
    }

    public class Heuristic : IHeuristicInterface<VisualTile>
    {
        private static readonly float SQRT_2 = Mathf.Sqrt(2);

        public float EstimateDistance(VisualTile current, VisualTile end)
        {
            var dx = Mathf.Abs(current.Coordinates.x - end.Coordinates.x);
            var dy = Mathf.Abs(current.Coordinates.y - end.Coordinates.y);

            return (Mathf.Min(dx, dy) * SQRT_2 + Math.Abs(dx - dy));
        }
    }
}
