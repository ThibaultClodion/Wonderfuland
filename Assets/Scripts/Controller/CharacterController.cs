using System.Collections.Generic;
using System.IO;
using UI;
using UnityEngine;

namespace Controller
{
    class CharacterController : MonoBehaviour
    {
        [HideInInspector] public List<Character> characters { protected set; get; } = new List<Character>();
        [HideInInspector] public List<GameObject> charactersGO { protected set; get; } = new List<GameObject>();
        [HideInInspector] public List<Rigidbody> charactersGORigidbody { protected set; get; } = new List<Rigidbody>();
        [HideInInspector] public List<bool> charactersCanPlay { protected set; get; } = new List<bool>();

        protected int InstantiateCharacter(CharacterData characterData, Character.CharacterTeam team, Vector2Int position)
        {
            GameObject characterGO = null;

            string json = File.ReadAllText(CharactersLoader.path + characterData.filePath);
            Character character = Character.GenerateFromJson(json, team);
            character.position = position;
            CharacterManager.AddCharacter(character);

            if (GameState.actualState == GameState.State.InCombat || GameState.actualState == GameState.State.ChoosingSpawnPosition)
            {
                characterGO = Instantiate(characterData.appearanceGO);
                MapManager.Instance.PutGameObjectOnCurrentMap(characterGO, position, characterGO.transform.position.y);
            }
            else
            {
                characterGO = Instantiate(characterData.appearanceGO, new Vector3(position.x, characterData.appearanceGO.transform.position.y, position.y), Quaternion.identity);
            }

            characters.Add(character);
            charactersGORigidbody.Add(characterGO.GetComponent<Rigidbody>());
            charactersGO.Add(characterGO);
            charactersCanPlay.Add(true);

            return characters.Count - 1;
        }

        protected void DestroyCharacter(int index)
        {
            if (charactersGO[index] != null)
            {
                Destroy(charactersGO[index]);
            }
            characters.RemoveAt(index);
            charactersGO.RemoveAt(index);
            charactersGORigidbody.RemoveAt(index);
            charactersCanPlay.RemoveAt(index);
        }

        public void Clear()
        {
            for (int i = characters.Count - 1; i >= 0; i--)
            {
                DestroyCharacter(i);
            }
            //characters.Clear();
            //charactersGO.RemoveAt(index);
            //charactersGORigidbody.RemoveAt(index);
        }
    }
}