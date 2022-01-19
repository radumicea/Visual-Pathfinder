using UnityEngine;
using UnityEngine.UI;

public class ToggleHandler : MonoBehaviour
{
    private static Color toggledColor = new Color(215F / 255F, 215F / 255F, 215F / 255F);

    private static Button on;

    [SerializeField] private Button setStart;
    [SerializeField] private Button setIPs;
    [SerializeField] private Button destroyTiles;

    public void OnButtonClicked(Button clickedButton)
    {
        if (clickedButton.Equals(on))
        {
            clickedButton.image.color = Color.white;

            on = null;
        }

        else
        {
            if (on != null)
            {
                on.image.color = Color.white;
            }

            clickedButton.image.color = toggledColor;

            on = clickedButton;
        }
    }

    public void SetStart()
    {
        MainVisualizer.SetStart();
        OnButtonClicked(setStart);
    }

    public void SetIPs()
    {
        MainVisualizer.SetInterestPoints();
        OnButtonClicked(setIPs);
    }

    public void DestroyTiles()
    {
        MainVisualizer.DestroyTiles();
        OnButtonClicked(destroyTiles);
    }
}