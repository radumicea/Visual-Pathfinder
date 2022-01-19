using UnityEngine;
using TMPro;

namespace UIHandlerNamespace
{
    public class DropdownHandler : MonoBehaviour
    {
        public void AlgorithmPicker()
        {
            var dropdown = GetComponentInParent<TMP_Dropdown>();

            switch (dropdown.value)
            {
                case 1:
                    MainVisualizer.Dijkstra();
                    dropdown.value = 0;
                    break;

                case 2:
                    MainVisualizer.AStar();
                    dropdown.value = 0;
                    break;
            }
        }

        public void ClearPicker()
        {
            var dropdown = GetComponentInParent<TMP_Dropdown>();

            switch (dropdown.value)
            {
                case 1:
                    MainVisualizer.ClearPaths();
                    dropdown.value = 0;
                    break;

                case 2:
                    MainVisualizer.ClearInterestPoints();
                    dropdown.value = 0;
                    break;

                case 3:
                    MainVisualizer.ClearDestruction();
                    dropdown.value = 0;
                    break;

                case 4:
                    MainVisualizer.ClearTiles();
                    dropdown.value = 0;
                    break;
            }
        }
    }
}