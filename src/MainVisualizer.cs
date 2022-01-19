// TODO:
// more algs

using System.Collections.Generic;

using UnityEngine;

using UIHandlerNamespace;
using VisualGridNamespace;
using VisualPathFinderNamespace;

public class MainVisualizer : MonoBehaviour
{
    private const int resizeFactor = 2;

    private static int vertical;
    private static int horizontal;

    private static VisualPathFinder pathFinder;
    private static readonly Heuristic heuristic = new Heuristic();
    private static UnsetErrorDisplayer ued;

    internal static VisualTile Start = null;
    private static bool setStart = false;
    private static HashSet<VisualTile> interestPoints = new HashSet<VisualTile>();
    internal static VisualTile Closest = null;
    private static bool setIPs = false;
    private static bool destroyTiles = false;

    public static VisualPathFinder PathFinder { get { return pathFinder; } }
    public static IHeuristicInterface<VisualTile> Heuristic { get { return heuristic; } }
    public static bool SetStartFlag { get { return setStart; } }
    public static HashSet<VisualTile> InterestPoints { get { return interestPoints; } }
    public static bool SetIPsFlag { get { return setIPs; } }
    public static bool DestroyTilesFlag { get { return destroyTiles; } }

    [SerializeField] private Transform camTrans;

    private void Awake()
    {
        VisualGrid.ResizeFactor = resizeFactor;

        var fVertical = 2 * Camera.main.orthographicSize * resizeFactor;
        vertical = (int)fVertical;
        horizontal = (int)(fVertical * Camera.main.aspect);
        var dv = (vertical <= horizontal) ? -vertical / 6F / resizeFactor : vertical / 10.25F / resizeFactor;
        var verticalOffset = (vertical > horizontal) ? 0 : 0.5F;
        vertical -= (int)Mathf.Abs(dv);

        VisualGrid.Vertical = vertical;
        VisualGrid.Horizontal = horizontal;

        ButtonCreationHandler.SetButtonsPosition(vertical, horizontal);

        var tmp = new GameObject();
        pathFinder = tmp.AddComponent<VisualPathFinder>();
        pathFinder.SetHeuristic<Heuristic, VisualTile>(heuristic);
        ued = tmp.AddComponent<UnsetErrorDisplayer>();

        camTrans.transform.position = new Vector3((horizontal / 2F - 0.5F) / resizeFactor, ((vertical - dv) / 2F - verticalOffset) / resizeFactor, -10 * resizeFactor);
    }

    public static void AStar()
    {
        if (pathFinder.IsSearching)
        {
            return;
        }

        if (Start != null && interestPoints.Count > 0)
        {
            pathFinder.AStar(VisualGrid.GridGraph, Start, interestPoints);
        }
        else
        {
            ued.Display();
        }
    }

    public static void Dijkstra()
    {
        if (pathFinder.IsSearching)
        {
            return;
        }

        if (Start != null && interestPoints.Count > 0)
        {
            pathFinder.Dijkstra(VisualGrid.GridGraph, Start, interestPoints);
        }
        else
        {
            ued.Display();
        }
    }

    public static void DestroyTiles()
    {
        setStart = false;
        setIPs = false;
        destroyTiles = !destroyTiles;
    }

    public static void SetStart()
    {
        setIPs = false;
        destroyTiles = false;
        setStart = !setStart;
    }

    public static void SetInterestPoints()
    {
        setStart = false;
        destroyTiles = false;
        setIPs = !setIPs;
    }

    public static void ClearTiles()
    {
        Start = null;
        interestPoints = new HashSet<VisualTile>();
        pathFinder.Stop();

        for (var i = 0; i < vertical; i++)
        {
            for (var j = 0; j < horizontal; j++)
            {
                VisualGrid.GridMap[i][j].ClearTile();
            }
        }
    }

    public static void ClearInterestPoints()
    {
        pathFinder.Stop();

        foreach (var tile in interestPoints)
        {
            tile.ClearTile();
        }

        interestPoints = new HashSet<VisualTile>();
    }

    public static void ClearDestruction()
    {
        pathFinder.Stop();

        for (var i = 0; i < vertical; i++)
        {
            for (var j = 0; j < horizontal; j++)
            {
                VisualGrid.GridMap[i][j].ClearDestroyed();
            }
        }
    }

    public static void ClearPaths()
    {
        pathFinder.Stop();

        for (var i = 0; i < vertical; i++)
        {
            for (var j = 0; j < horizontal; j++)
            {
                VisualGrid.GridMap[i][j].ClearColor();
            }
        }
    }

    public static void SetSpeed(int speed)
    {
        pathFinder.Speed = speed;
    }
}
