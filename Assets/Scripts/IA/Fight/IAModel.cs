using SpellCreator;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace IA.Fight
{
    class IAModel
    {
        public List<Character> characters;

        public IAModel()
        {
            characters = new List<Character>();
        }

        public IAModel(List<Character> _characters)
        {
            characters = _characters;
        }

        public void Play()
        {
            foreach (Character character in characters)
            {
                Character targetedEnnemy = GetTarget(character);
                if (targetedEnnemy == null)
                    return;

                bool launchSpell;
                do
                {
                    launchSpell = false;
                    foreach (int spellIndex in GetSpellIndexByEffect<Damage>(character))
                    {
                        if (character.CanLaunchSpellAt(spellIndex, targetedEnnemy.position))
                        {
                            character.LaunchSpell(spellIndex, targetedEnnemy.position);
                            launchSpell = true;
                            break;
                        }
                        else if (character.CanLaunchSpell(spellIndex))
                        {
                            List<int> movementSpellsIndex = GetSpellIndexByEffect<Movement>(character);
                            if (movementSpellsIndex.Count == 0)
                                return;

                            int movementSpellIndex = movementSpellsIndex[0]; //TMP
                            List<Vector2Int> path = AStar.execute(MapManager.Instance.GetGrid(), character.position, targetedEnnemy.position);
                            path.Reverse();
                            foreach (Vector2Int launchPosition in path)
                            {
                                if (character.CanLaunchSpellAt(movementSpellIndex, launchPosition))
                                {
                                    character.LaunchSpell(movementSpellIndex, launchPosition);
                                    launchSpell = true;
                                    break;
                                }
                            }
                        }
                    }
                } while (launchSpell);
            }
        }

        //Utils
        private Character GetTarget(Character character)
        {
            Character targetedEnnemy = null;
            foreach (Character ennemy in CharacterManager.GetCharacters())
            {
                if (ennemy.team == character.team)
                {
                    continue;
                }

                targetedEnnemy = ennemy;
                break;
            }
            return targetedEnnemy;
        }

        private List<int> GetSpellIndexByEffect<T>(Character character)
        {
            List<int> result = new List<int>();
            for (int spellIndex = 0; spellIndex < character.spellDeck.spells.Count; spellIndex++)
            {
                if (character.spellDeck.spells[spellIndex].effects.Count == 0)
                    continue;
                if (character.spellDeck.spells[spellIndex].effects[0].GetType() != typeof(T))
                    continue;
                result.Add(spellIndex);
            }
            return result;
        }
    }
}