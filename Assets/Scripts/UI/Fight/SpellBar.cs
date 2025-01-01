using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    class SpellBar : MonoBehaviour
    {
        public Button buttonGO;

        private List<GameObject> instantiateButtons = new List<GameObject>();
        private Action<int> functionOnClick;
        private int currentSelection = -1;
        private Color selectedColor = Color.gray;
        private Color unselectedColor = Color.white;

        public void Initialize(Action<int> _functionOnClick, Character character) // Plutot avoir la liste des spells
        {
            if (buttonGO == null)
            {
                Debug.LogError("buttonGO is null");
                return;
            }

            functionOnClick = _functionOnClick;
            foreach(Transform child in transform) 
            { 
                Destroy(child.gameObject);
            }
            instantiateButtons.Clear();

            for (int i = 0; i < character.spellDeck.spells.Count; i++)
            {
                GameObject instantiateGO = Instantiate(buttonGO.gameObject, transform);
                Button button = instantiateGO.GetComponent<Button>();

                Image spellImage = button.GetComponent<Image>();
                string iconPath = Application.dataPath + Path.DirectorySeparatorChar + character.spellDeck.spells[i].iconPath;
                if (spellImage != null && File.Exists(iconPath))
                {
                    byte[] FileData = File.ReadAllBytes(iconPath);
                    Texture2D Tex2D = new Texture2D(2, 2);
                    if (Tex2D.LoadImage(FileData))
                    {
                        spellImage.sprite = Sprite.Create(Tex2D, new Rect(0, 0, Tex2D.width, Tex2D.height), new Vector2(0, 0), 100);
                    }   
                }

                int copy = i; // l'Action en delegate prend la valeur mémoire / Si on utilise i on a functionOnClick(spells.Count + 1)
                button.onClick.AddListener(delegate { UpdateSelectedSpellColor(copy); });
                instantiateButtons.Add(instantiateGO);
            }
        }

        public void UpdateSelectedSpellColor(int index)
        {
            if(instantiateButtons.Count == 0) 
                return;
            
            if(currentSelection != -1)
            {
                instantiateButtons[currentSelection].GetComponent<Image>().color = unselectedColor;
                if (currentSelection == index)
                {
                    currentSelection = -1;
                    return;
                }
            }

            functionOnClick(index);
            currentSelection = index;
            instantiateButtons[currentSelection].GetComponent<Image>().color = selectedColor; 
        }

        public void UnselectSpell()
        {
            if (currentSelection != -1)
            {
                instantiateButtons[currentSelection].GetComponent<Image>().color = unselectedColor;
                currentSelection = -1;
            }
        }
    }
}
