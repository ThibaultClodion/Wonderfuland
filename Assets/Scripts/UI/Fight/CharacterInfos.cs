using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    class CharacterInfos : MonoBehaviour
    {
        public TextMeshProUGUI lifeText;
        public TextMeshProUGUI PAText;
        public Image iconGO;

        public void Display(Character character)
        {
            lifeText.text = character.statsCurrent.Life.ToString();
            PAText.text = character.statsCurrent.PA.ToString();
            
            string iconPath = Application.dataPath + Path.DirectorySeparatorChar + character.iconPath;
            if (File.Exists(iconPath))
            {
                byte[] FileData = File.ReadAllBytes(iconPath);
                Texture2D Tex2D = new Texture2D(2, 2);
                if (Tex2D.LoadImage(FileData))
                {
                    iconGO.sprite = Sprite.Create(Tex2D, new Rect(0, 0, Tex2D.width, Tex2D.height), new Vector2(0, 0), 100);
                }
            }
        }

    }
}