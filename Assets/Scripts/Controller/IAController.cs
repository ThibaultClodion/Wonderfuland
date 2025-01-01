using IA.Fight;
using System.Collections.Generic;
using UnityEngine;

namespace Controller
{
    class IAController : CharacterController
    {
        private Dictionary<Character.CharacterTeam, IAModel> models;
        private Character.CharacterTeam playingTeam;
        private bool canPlay = false;

        public void Awake()
        {
            models = new Dictionary<Character.CharacterTeam, IAModel>
        {
            { Character.CharacterTeam.BLUE, new IAModel() },
            { Character.CharacterTeam.RED, new IAModel() }
        };
        }

        private void Update()
        {
            if (canPlay)
            {
                if (models.ContainsKey(playingTeam))
                {
                    models[playingTeam].Play();
                }
                if (playingTeam == Character.CharacterTeam.RED)
                    FightManager.Instance.EndCurrentTurn();
                canPlay = false;
            }


            for (int i = 0; i < characters.Count; i++)
            {
                if (characters[i].statsCurrent.Life <= 0)
                {
                    DestroyCharacter(i);
                    i--;
                }
            }
        }

        public void Move(Vector2Int currentPosition, Vector2Int newPosition)
        {
            for (int i = 0; i < characters.Count; i++)
            {
                if (characters[i].position == currentPosition)
                {
                    MovementManager.MoveCharacter(newPosition, this, i);
                    return;
                }
            }
        }

        public void Play(Character.CharacterTeam team)
        {
            canPlay = true;
            playingTeam = team;
        }

        private void DestroyCharacter(Character.CharacterTeam team, int index)
        {
            DestroyCharacter(index);
            models[team].characters.RemoveAt(index);
        }

        public void InstantiateCharacter(List<CharacterData> characterDatas, Character.CharacterTeam team, List<Vector2Int> positions)
        {
            if (characterDatas.Count != positions.Count)
            {
                Debug.LogError("Mismatch between characterDatas and positions sizes");
            }

            for (int i = 0; i < characterDatas.Count; i++)
            {
                InstantiateCharacter(characterDatas[i], team, positions[i]);
            }
        }

        public new void InstantiateCharacter(CharacterData characterData, Character.CharacterTeam team, Vector2Int position)
        {
            int index = base.InstantiateCharacter(characterData, team, position);
            models[team].characters.Add(characters[index]);
        }
    }
}