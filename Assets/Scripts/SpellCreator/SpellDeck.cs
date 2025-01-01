using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;

namespace SpellCreator
{
    class SpellDeck
    {
        [JsonIgnore] public static int HAND_SIZE = 8;
        [JsonIgnore] public List<Spell> spells;
        public List<Spell> deck;

        public SpellDeck()
        {
            deck = new List<Spell>();
            spells = new List<Spell>();
        }

        public void Launch(int spellIndex, Character launcher, Vector2Int position)
        {
            spells[spellIndex].Launch(launcher, position);
        }

        public void InitEmpty(int numberOfSpells)
        {
            deck = new List<Spell>(numberOfSpells);
            for (int i = 0; i < numberOfSpells; i++)
                deck.Add(new Spell());

            Init();
        }

        [OnDeserialized]
        private void JsonInit(StreamingContext context)
        {
            Init();
        }
        
        public void Init()
        {
            spells.Clear();
            int spellSize = Math.Min(deck.Count, HAND_SIZE);
            for (int i = 0; i < spellSize;i++)
            {
                spells.Add(deck[i]);
            }
        }

        public void NextTurn()
        {
            foreach (Spell spell in deck)
            {
                spell.NextTurn();
            }
        }
    }
}
