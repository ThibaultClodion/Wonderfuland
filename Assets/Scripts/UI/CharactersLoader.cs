using System.Collections;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    class CharactersLoader : MonoBehaviour // peut etre générique
    {
        public static string path = Application.dataPath + "/Ressources/SpellCreator/";
        public CharacterCreatorUI spellCreatorUI;
        public Button buttoncharacterGO;
        
        void Awake()
        {
            ReloadCharacters();
        }

        public void ReloadCharacters()
        {
            foreach(Transform child in transform)
            {
                Destroy(child.gameObject);
            }

            DirectoryInfo loadedDirectory = new DirectoryInfo(path);
            foreach (FileInfo file in loadedDirectory.GetFiles())
            {
                string characterName = file.Name;
                if (file.Name.Substring(file.Name.Length - ".json".Length) != ".json")
                {
                    continue;
                }

                if (file.Name.IndexOf(".") != -1)
                {
                    characterName = file.Name.Remove(file.Name.IndexOf("."));
                }

                Button instanciateButtonGO = Instantiate(buttoncharacterGO, transform);
                TextMeshProUGUI instanciateTextGO = instanciateButtonGO.GetComponentInChildren<TextMeshProUGUI>();
                instanciateTextGO.text = characterName;
                instanciateButtonGO.onClick.AddListener(delegate {
                    spellCreatorUI.LoadCharacter(file.Name);
                    gameObject.SetActive(false);
                });
            }
        }
    }
}