using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Utils
{
    public class SliderValuePrinter : MonoBehaviour
    {
        public TextMeshProUGUI printedTextGO; // TextMeshPro
        private Slider slider;

        void Awake()
        {
            slider = GetComponent<Slider>();
            if(slider != null && printedTextGO != null)
            {
                printedTextGO.text = ((int)slider.value).ToString();
                slider.onValueChanged.AddListener(delegate { UpdateValuePrinted(); });
            }
        }

        private void UpdateValuePrinted()
        {
            printedTextGO.GetComponent<TextMeshProUGUI>().text = ((int)slider.value).ToString();
        }
    }
}