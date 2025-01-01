using Newtonsoft.Json;
using SpellCreator;
using System.IO;
using TMPro;
using UI.Utils;
using UnityEngine;

namespace UI
{
    class CharacterCreatorUI : MonoBehaviour
    {
        private Character character;

        public ReflectionPanel characterPanelGO;
        public TMP_InputField characterNameGO;
        public SpellCreatorUI effectCreator;
        
        private void Awake()
        {
            character = Character.GenerateFromJson("", Character.CharacterTeam.BLUE);
            character.spellDeck.InitEmpty(SpellDeck.HAND_SIZE);
            characterPanelGO.Instantiate<Stats>(UpdateStatFromCharacter);

            effectCreator.Initialize(ref character);
        }

        // Events
        // Save Button
        public void SaveCharacter()
        {
            string path = CharactersLoader.path;
            
            if (Directory.Exists(path))
            {
                path += characterNameGO.text + ".json";
                if (!string.IsNullOrEmpty(characterNameGO.text))// && !File.Exists(path))
                {
                    string json = JsonConvert.SerializeObject(character, Character.jsonSettings);
                    File.WriteAllText(path, json);
                }
                else
                {
                    Debug.Log("Path : " + path + " already exists");
                }
            }
            else
            {
                Debug.Log("Path : " + path + " doesn't exists");
            }
        }

        // Load Button
        public void LoadCharacter(string fileName)
        {
            string json = File.ReadAllText(CharactersLoader.path + fileName);
            character = Character.GenerateFromJson(json, Character.CharacterTeam.BLUE);
            characterPanelGO.UpdateValues(ref character.statsInit);
            
            effectCreator.Initialize(ref character);
            characterNameGO.text = fileName.Replace(".json", "");
        }

        // On generated stat fields update
        public void UpdateStatFromCharacter(string fieldName)
        {
            characterPanelGO.UpdateField(ref character.statsInit, fieldName);
        } 
    }
}
