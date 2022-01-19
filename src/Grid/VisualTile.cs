using UnityEngine;
using UnityEngine.EventSystems;
using VisualPathFinderNamespace;

namespace VisualGridNamespace
{
    public class VisualTile : MonoBehaviour, IVisualNodeInterface
    {
        private static readonly Color firstColor = new Color(253F / 255F, 253F / 255F, 253F / 255F);
        private static readonly Color secondColor = new Color(235F / 255F, 235 / 255F, 235 / 255F);
        private static readonly Color ipColor = new Color(203 / 255F, 174 / 255F, 69 / 255F);
        private static readonly Color pathColor = new Color(1, 241 / 255F, 3 / 255F, 0.7F);
        private static readonly Color startColor = new Color(203 / 255F, 107 / 255F, 69 / 255F);
        private static readonly Color candidateColor = new Color(0, 173 / 255F, 173 / 255F, 0.7F);
        private static readonly Color candidateGradientColor = new Color(0 / 255F, 173 / 255F, 0 / 255F, 0.7F);
        private static readonly Color visitedColor = new Color(77 / 255F, 206 / 255F, 226 / 255F, 0.7F);
        private static readonly Color visitedGradientColor = new Color(77 / 255F, 226 / 255F, 97 / 255F, 0.7F);
        private static readonly Color closestColor = new Color(195 / 255F, 69 / 255F, 203 / 255F);
        private static readonly Color voidColor = new Color(0, 0, 0, 0.7F);

        private static float biggestDistance;

        [SerializeField] private SpriteRenderer sRend;
        [SerializeField] private GameObject tileHighlight;
        [SerializeField] private GameObject tileColor;

        private bool isAvailable;
        private Vector2Int coordinates;

        public bool IsAvailable { get { return isAvailable; } set { isAvailable = value; } }
        public Vector2Int Coordinates { get { return coordinates; } }

        public void Init(int i, int j)
        {
            isAvailable = true;
            coordinates = new Vector2Int(i, j);
            sRend.color = ((i + j) % 2 == 0) ? firstColor : secondColor;
        }

        public bool CanGoTo(GraphNamespace.INodeInterface n)
        {
            VisualTile tile = (VisualTile)n;

            if ((coordinates.x - 1 < 0 || VisualGrid.GridMap[coordinates.x - 1][coordinates.y].isAvailable == false) &&
                (coordinates.y + 1 >= VisualGrid.Horizontal || VisualGrid.GridMap[coordinates.x][coordinates.y + 1].isAvailable == false) &&
                tile.coordinates.x == coordinates.x - 1 && tile.coordinates.y == coordinates.y + 1)
            {
                return false;
            }

            if ((coordinates.y - 1 < 0 || VisualGrid.GridMap[coordinates.x][coordinates.y - 1].isAvailable == false) &&
                (coordinates.x - 1 < 0 || VisualGrid.GridMap[coordinates.x - 1][coordinates.y].isAvailable == false) &&
                tile.coordinates.x == coordinates.x - 1 && tile.coordinates.y == coordinates.y - 1)
            {
                return false;
            }

            if ((coordinates.y - 1 < 0 || VisualGrid.GridMap[coordinates.x][coordinates.y - 1].isAvailable == false) &&
                (coordinates.x + 1 >= VisualGrid.Vertical || VisualGrid.GridMap[coordinates.x + 1][coordinates.y].isAvailable == false) &&
                tile.coordinates.x == coordinates.x + 1 && tile.coordinates.y == coordinates.y - 1)
            {
                return false;
            }

            if ((coordinates.y + 1 >= VisualGrid.Horizontal || VisualGrid.GridMap[coordinates.x][coordinates.y + 1].isAvailable == false) &&
                (coordinates.x + 1 >= VisualGrid.Vertical || VisualGrid.GridMap[coordinates.x + 1][coordinates.y].isAvailable == false) &&
                tile.coordinates.x == coordinates.x + 1 && tile.coordinates.y == coordinates.y + 1)
            {
                return false;
            }

            return true;
        }

        public void MarkClosest()
        {
            if (tileColor.activeSelf == true && (MainVisualizer.Start == this || tileColor.GetComponent<SpriteRenderer>().color == pathColor))
            {
                return;
            }

            tileColor.SetActive(true);
            tileColor.GetComponent<SpriteRenderer>().color = closestColor;
            MainVisualizer.Closest = this;
            biggestDistance = 0;
        }

        public void MarkCandidate()
        {
            if (tileColor.activeSelf == true &&
                (MainVisualizer.Start == this || tileColor.GetComponent<SpriteRenderer>().color == pathColor ||
                 MainVisualizer.InterestPoints.Contains(this)))
            {
                return;
            }

            tileColor.SetActive(true);
            var gradient = GetGradient();
            tileColor.GetComponent<SpriteRenderer>().color = (1 - gradient) * candidateColor + gradient * candidateGradientColor;
        }

        public void MarkVisited()
        {
            if (tileColor.activeSelf == true &&
                (MainVisualizer.Start == this || tileColor.GetComponent<SpriteRenderer>().color == pathColor ||
                 MainVisualizer.InterestPoints.Contains(this)))
            {
                return;
            }

            tileColor.SetActive(true);
            var gradient = GetGradient();
            tileColor.GetComponent<SpriteRenderer>().color = (1 - gradient) * visitedColor + gradient * visitedGradientColor;
        }

        public void MarkPath()
        {
            if (tileColor.activeSelf == true &&
                (MainVisualizer.Start == this ||
                 MainVisualizer.InterestPoints.Contains(this)))
            {
                return;
            }

            tileColor.SetActive(true);
            tileColor.GetComponent<SpriteRenderer>().color = pathColor;
        }

        private void OnMouseEnter()
        {
            tileHighlight.SetActive(true);

            if (EventSystem.current.IsPointerOverGameObject())
            {
                return;
            }

            if (MainVisualizer.PathFinder.IsSearching)
            {
                return;
            }

            if (MainVisualizer.DestroyTilesFlag && Input.GetMouseButton(0))
            {
                if (MainVisualizer.Start == this ||
                    MainVisualizer.InterestPoints.Contains(this))
                {
                    return;
                }

                tileColor.SetActive(true);
                tileColor.GetComponent<SpriteRenderer>().color = voidColor;
                VisualGrid.MarkUnavailable(coordinates.x, coordinates.y);
            }

            if (isAvailable == false)
            {
                return;
            }
        }

        private void OnMouseExit()
        {
            tileHighlight.SetActive(false);
        }

        private void OnMouseDown()
        {
            if (EventSystem.current.IsPointerOverGameObject())
            {
                return;
            }

            if (MainVisualizer.PathFinder.IsSearching)
            {
                return;
            }

            if (MainVisualizer.DestroyTilesFlag)
            {
                if (MainVisualizer.Start == this ||
                    MainVisualizer.InterestPoints.Contains(this))
                {
                    return;
                }

                tileColor.SetActive(true);
                tileColor.GetComponent<SpriteRenderer>().color = voidColor;
                VisualGrid.MarkUnavailable(coordinates.x, coordinates.y);
            }
            
            else if (isAvailable == false)
            {
                return;
            }

            else if (MainVisualizer.SetStartFlag &&
                     MainVisualizer.InterestPoints.Contains(this) == false)
            {
                if (MainVisualizer.Start != null)
                {
                    MainVisualizer.Start.tileColor.SetActive(false);
                }

                MainVisualizer.Start = this;
                tileColor.SetActive(true);
                tileColor.GetComponent<SpriteRenderer>().color = startColor;
            }

            else if (MainVisualizer.SetIPsFlag && MainVisualizer.Start != this &&
                     MainVisualizer.InterestPoints.Contains(this) == false)
            {
                MainVisualizer.InterestPoints.Add(this);
                tileColor.SetActive(true);
                tileColor.GetComponent<SpriteRenderer>().color = ipColor;
            }
        }

        public void ClearTile()
        {
            tileColor.SetActive(false);
            isAvailable = true;
        }

        public void ClearDestroyed()
        {
            if (isAvailable == false)
            {
                tileColor.SetActive(false);
                isAvailable = true;
            }
        }

        public void ClearColor()
        {
            if (isAvailable && this != MainVisualizer.Start)
            {
                if (MainVisualizer.InterestPoints.Contains(this))
                {
                    tileColor.GetComponent<SpriteRenderer>().color = ipColor;
                }
                else
                {
                    tileColor.SetActive(false);
                }
            }
        }

        private float GetGradient()
        {
            if (MainVisualizer.Closest == null)
            {
                return 0F;
            }

            var estimatedDistance = MainVisualizer.Heuristic.EstimateDistance(this, MainVisualizer.Closest);

            if (biggestDistance == 0)
            {
                biggestDistance = estimatedDistance;
            }

            // Normalize between 0 (when distance is biggest) and 1, when distance is smallest.
            return -1F / biggestDistance * (estimatedDistance - biggestDistance);
            // Will also return negative when estDist > start dist.
        }
    }
}