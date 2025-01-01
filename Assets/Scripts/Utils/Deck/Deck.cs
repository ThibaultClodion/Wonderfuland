using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Utils.Deck
{
    // Permet d'avoir une main fixe
    class EmptyDeck<T>
    {
        [JsonIgnore] public const int DEFAULT_HAND_SIZE = 8;
        [JsonIgnore] public List<T> hand { protected set; get; }
        protected List<T> deck;
        public int handSize;

        public EmptyDeck(int _handSize, List<T> _deck)
        {
            deck = _deck;
            handSize = _handSize;
            hand = new List<T>(handSize);
        }

        public virtual void Launch(int index)
        {

        }

        [OnSerialized]
        public virtual void InitHand()
        {
            hand.Clear();
            for (int i = 0; i < handSize; i++)
            {
                hand.Add(deck[i]);
            }
        }

        // Only useful for creation of a character
        public void InitEmptyHand(int numberOfSpells)
        {
            deck = new List<T>(numberOfSpells);
            for (int i = 0; i < numberOfSpells; i++)
            {
#nullable enable // Macro warning CS8632
                T? t = default;
                deck.Add(t);
            }

            InitHand();
        }

        public virtual void NextTurn()
        {

        }
    }

    class Deck<T> : EmptyDeck<T>
    {
        // TODO : Ajouter plus d'optins pour le deck -> Cartes de la main partent seulement à la fin du tour (les 2 a gauches...) / Avoir une autre taille de main...
        // Ajouter des poids sur les cartes s'il n'y a pas de défausse
        public bool shuffle;
        public int drawPerTurn;
        private int drawIndex;
        private bool leaveHandAtEnd = true;
        [JsonIgnore] public List<T> discardCards { private set; get; }

        public Deck(int _handSize, List<T> _deck) : base(_handSize, _deck)
        {
            discardCards = new List<T>();
        }


        public override void Launch(int index)
        {
            discardCards.Add(hand[index]);
            hand.RemoveAt(index);
        }

        [OnSerialized]
        public override void InitHand()
        {
            discardCards.Clear();
            if (shuffle)
            {
                Shuffle();
            }

            base.InitHand();
            drawIndex = handSize;
        }

        protected void Shuffle()
        {
            for (int i = 0; i < deck.Count; i++)
            {
                Random rand = new Random();

                int rand1 = rand.Next(deck.Count);
                int rand2 = rand.Next(deck.Count);

                T spellBuffer = deck[rand1];
                deck[rand1] = deck[rand2];
                deck[rand2] = spellBuffer;
            }
        }

        protected void Draw()
        {
            hand.Add(deck[drawIndex]);
            drawIndex++;
        }

        private void AddDiscardToDeck()
        {
            if (leaveHandAtEnd)
            {
                InitHand();
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        public override void NextTurn()
        {
            for (int i = 0; i < drawPerTurn; i++)
            {
                if (hand.Count < handSize)
                {
                    Draw();
                }

                if (deck.Count - drawIndex == 0)
                {
                    AddDiscardToDeck();
                }
            }
        }
    }
}
