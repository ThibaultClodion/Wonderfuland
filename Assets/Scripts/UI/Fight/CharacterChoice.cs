using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

class CharacterChoice : MonoBehaviour
{
    public Button buttonGO;
    private List<GameObject> instantiateButtons = new List<GameObject>();

    public void Initialize(Action<int> _functionOnClick, List<Character> characters)
    {
        if (buttonGO == null)
        {
            Debug.LogError("buttonGO is null");
            return;
        }

        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }
        instantiateButtons.Clear();

        for (int i = 0; i < characters.Count; i++)
        {
            GameObject instantiateGO = Instantiate(buttonGO.gameObject, transform);
            Button button = instantiateGO.GetComponent<Button>();

            Image spellImage = button.GetComponent<Image>();
            string iconPath = Application.dataPath + Path.DirectorySeparatorChar + characters[i].iconPath;
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
            button.onClick.AddListener(delegate { _functionOnClick(copy); });
            instantiateButtons.Add(instantiateGO);
        }
    }
}
