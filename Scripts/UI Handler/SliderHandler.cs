using UnityEngine;
using UnityEngine.UI;

public class SliderHandler : MonoBehaviour
{
    private Slider slider;

    private void Start()
    {
        slider = GetComponentInParent<Slider>();
    }

    public void SetSpeed()
    {
        MainVisualizer.SetSpeed((int)slider.value);
    }
}
