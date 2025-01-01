using IA.Fight;
using System;
using System.IO;
using UI;
using UnityEngine;
using Utils;
using static SpellCreator.Alteration;

namespace SpellCreator
{
    //Penser aux effets en plusieurs parties -> Par exemple on lance sur un joueur puis sur une case vide pour un effet à appliquer au jouer 
    class Invocation : Effect
    {
        public string characterDataPath;
        bool canBePlayed;//public
        //ModelParameters modelParameters;
        [ReflectionLimit(0, 100)] public int lifeLinkPercent; // pas besoin (a implémenter dans etat

        public override void Apply(Character launcher, Vector2Int position)
        {
            CharacterData characterData = Resources.Load("Characters/" + characterDataPath) as CharacterData;
            PlayerManager.IAController.InstantiateCharacter(characterData, launcher.team, position);
        }
    }

    class MapAlteration : Effect
    {
        Cell.CellType type;

        public override void Apply(Character launcher, Vector2Int position)
        {
            FightData.UpdateMapCellType(type, position.x, position.y);
        }
    }

    class IAAlteration : Effect
    {

    }

    class DeckAlteration : Effect
    {

    }

    class Transformation : Effect
    {
        public string characterDataPath;
        //bool keepEtats;

        public override void Apply(Character launcher, Vector2Int position)
        {
            int lifeRatio = launcher.statsCurrent.Life / launcher.statsInit.Life;
            string json = File.ReadAllText(CharactersLoader.path + characterDataPath);
            //CharacterData characterData = Resources.Load("Characters/" + characterDataPath) as CharacterData;

            Character character = Character.GenerateFromJson(json, launcher.team);
            character.statsCurrent.Life = launcher.statsInit.Life * lifeRatio;
            character.statsInit.PA = launcher.statsCurrent.PA;
            character.etats = launcher.etats;
        }
    }

    class Damage : Effect
    {
        [ReflectionLimit(0, 100)] public int damage;
        public bool lifesteal;
        [ReflectionLimit(0, 100)] public int lifestealPercent;

        public override void Apply(Character launcher, Vector2Int position)
        {
            Character target = FightData.GetCharacterByPosition(position);
            if (target == null)
            {
                Debug.Log("No character at this case");
                return;
            }

            target.statsCurrent.LoseStat(Stats.StatEnum.Life, damage);
            if(lifesteal)
            {
                int lifeIncrement = damage * (lifestealPercent / 100);
                int missingLife = launcher.statsInit.Life - launcher.statsCurrent.Life;
                launcher.statsCurrent.GainStat(Stats.StatEnum.Life, lifeIncrement > missingLife ? missingLife : lifeIncrement);
            }
        }
    }

    class EtatApply : Effect
    {
        public AlterationType alterationType;
        public Etat etatToApply;
        [ReflectionLimit(0, 2)] public int numberToApply;

        public override void Apply(Character launcher, Vector2Int position)
        {
            Character target = FightData.GetCharacterByPosition(position);
            if (target == null)
            {
                Debug.Log("No character at this case");
                return;
            }
                
            if (alterationType == AlterationType.APPLY)
            {
                target.AddEtat(etatToApply, numberToApply);
            }
            else if (alterationType == AlterationType.ABSORB)
            {
                target.AddEtat(etatToApply, -numberToApply);
                launcher.AddEtat(etatToApply, numberToApply);
            }
            else if (alterationType == AlterationType.REMOVE)
            {
                target.AddEtat(etatToApply, -numberToApply);
            }
            else
            {
                throw new NotImplementedException(alterationType + " doesn't implemented");
            }
        }
    }

    class Movement : Effect
    {
        public bool teleportToPosition;
        public Vector2Int centerPosition;
        public Vector2Int projectionToApply;

        public override void Apply(Character launcher, Vector2Int position)
        {
            if(teleportToPosition)
            {
                FightData.MoveCharacterTo(launcher.position, position);
            }
            else
            {
                FightData.MoveCharacterTo(position, position + projectionToApply);
            }
        }
    }

    class Alteration : Effect
    {
        public AlterationType alterationType;
        public Stats.StatEnum alterateStat;
        [ReflectionLimit(0, 50)] public int value; // dépend de la stat

        public enum AlterationType
        {
            ABSORB,
            APPLY,
            REMOVE
        }

        public override void Apply(Character launcher, Vector2Int position)
        {
            Character target = FightData.GetCharacterByPosition(position);
            if (target == null)
            {
                Debug.Log("No character at this case");
                return;
            }

            if (alterationType == AlterationType.APPLY)
            {
                target.statsCurrent.GainStat(alterateStat, value);
            }
            else if (alterationType == AlterationType.ABSORB)
            {
                launcher.statsCurrent.GainStat(alterateStat, value);
                target.statsCurrent.LoseStat(alterateStat, value);
            }
            else if (alterationType == AlterationType.REMOVE)
            {
                target.statsCurrent.LoseStat(alterateStat, value);
            }
            else
            {
                throw new NotImplementedException(alterationType + " doesn't implemented");
            }
        }
    }
}

