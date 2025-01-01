using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Utils
{
    public class BooleanButton : MonoBehaviour
    {
        public string trueText; // peut etre une image ou TextMesh
        [HideInInspector] public bool currentValue = false;
        
        private Button button;
        private string falseText;
        
        void Awake()
        {
            button = GetComponent<Button>();
            if(button != null)
            {
                TextMeshProUGUI textMeshPro = button.GetComponentInChildren<TextMeshProUGUI>();
                if (textMeshPro != null)
                {
                    falseText = textMeshPro.text;
                    button.onClick.AddListener(delegate { UpdateValuePrinted(); });
                }
            }
        }

        private void UpdateValuePrinted()
        {
            currentValue = !currentValue;
            if (currentValue)
            {
                button.GetComponentInChildren<TextMeshProUGUI>().text = trueText;
            }
            else
            {
                button.GetComponentInChildren<TextMeshProUGUI>().text = falseText;
            }
        }
    }
}