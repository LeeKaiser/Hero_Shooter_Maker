using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace HeroShooterMakerDemo
{
    public class UIBar : MonoBehaviour
    {
        public Slider BarSlider;
        public TextMeshProUGUI BarText;
        public bool ShowMaxValue;
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            BarSlider = GetComponent<Slider>();
        }

        public void UpdateSlider(int currentValue, int maxValue)
        {
            BarSlider.maxValue = maxValue;
            BarSlider.value = currentValue;
            string text = "" + currentValue;
            if (ShowMaxValue)
            {
                text = text + "/" + maxValue;
            }
            BarText.text = text;
        }
    }
}