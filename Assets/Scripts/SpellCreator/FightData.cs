using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace SpellCreator
{
    static class FightData
    {
        public static int turn;

        public static Character GetCharacterByPosition(Vector2Int position)
        {
            foreach (Character character in CharacterManager.GetCharacters())
            {
                if (character.position == position) return character;
            }
            return null;
        }

        //public static Character GetCharacterByGameID(int gameID)
        //{
        //    foreach (Character character in CharacterManager.GetCharacters())
        //    {
        //        if (character.gameId == gameID) return character;
        //    }
        //    return null;
        //}

        public static void MoveCharacterTo(Vector2Int currentPosition, Vector2Int newPosition)
        {
            for(int i = 0; i < PlayerManager.playerControllers.Count; i++)
            {
                Character character = PlayerManager.playerControllers[i].GetCurrentCharacter();
                if (character != null && character.position == currentPosition)
                {
                    PlayerManager.playerControllers[i].GridMove(newPosition);
                    return;
                }
            }

            PlayerManager.IAController.Move(currentPosition, newPosition);
        }

        public static void UpdateMapCellType(Cell.CellType type, int x, int y)
        {
            MapManager.Instance.GetGrid()[x][y].SetType(type);
        }
    }
}
