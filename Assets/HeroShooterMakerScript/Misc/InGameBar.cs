using UnityEngine;
using UnityEngine.UI;

public class InGameBar : MonoBehaviour
{
    public Slider BarSlider;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        BarSlider = GetComponent<Slider>();
    }

    public void UpdateSlider(int currentValue, int maxValue)
    {
        
    }
}
