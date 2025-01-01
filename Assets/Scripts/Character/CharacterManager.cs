using System.Collections.Generic;
using System.Linq;
using UnityEngine;

class CharacterManager
{
    //Current characters on Map
    private static List<Character> characters = new List<Character>();

    public static List<Character> GetCharacters()
    {
        return characters;
    }

    public static void AddCharacter(Character character)
    {
        characters.Add(character);
    }

    public static void RemoveCharacter(Character character)
    {
        characters.Remove(character);
    }

    public static void Clear()
    {
        characters.Clear();
    }

    public static List<Character> GetCharactersByTeam(Character.CharacterTeam team)
    {
        return characters.Where(x => x.team == team).ToList();
    }

    public static bool HasACharacterOnThisPosition(Vector2Int position)
    {
        foreach (Character character in characters)
        {
            if (character.position == position)
            {
                return true;
            }
        }

        return false;
    }
}
