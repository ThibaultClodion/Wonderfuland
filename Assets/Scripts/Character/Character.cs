using Newtonsoft.Json;
using SpellCreator;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

class Character
{
    [JsonIgnore]
    public Vector2Int position;
    [JsonIgnore]
    public int gameId;
    [JsonIgnore]
    public Stats statsCurrent;
    [JsonIgnore]
    public CharacterTeam team;
    [JsonIgnore]
    public Dictionary<Etat, int> etats = new Dictionary<Etat, int>();
    [JsonIgnore]
    public static JsonSerializerSettings jsonSettings = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All }; // Objects ?

    public string iconPath;
    public Stats statsInit = new Stats();
    public Passif passif = new Passif();
    public SpellDeck spellDeck = new SpellDeck();

    public enum CharacterTeam
    {
        BLUE,
        RED,
        YELLOW
    }

    private Character()
    {

    }

    private Character(Character character, CharacterTeam _team)
    {
        team = _team;
        position = Vector2Int.zero;
        gameId = 0;

        if (character != null)
        {
            statsInit = character.statsInit;
            statsCurrent = statsInit.Clone();
            passif = character.passif;
            spellDeck = character.spellDeck;
            iconPath = character.iconPath;
        }
    }

    public static Character GenerateFromJson(string json, CharacterTeam team)
    {
        return new Character(JsonConvert.DeserializeObject<Character>(json, jsonSettings), team);
    }

    public void LaunchSpell(int index, Vector2Int launchPosition)
    {
        if (!CanLaunchSpellAt(index, launchPosition))
        {
            return;
        }

        Spell spell = spellDeck.spells[index];
        statsCurrent.LoseStat(spell.costStat, spell.costValue);
        spell.Launch(this, launchPosition);
    }

    public void Move(Vector2Int newPosition)
    {
        position = newPosition;
    }

    public bool CanLaunchSpell(int index)
    {
        return spellDeck.spells[index].CanBeLaunched(statsCurrent);
    }

    public bool CanLaunchSpellAt(int index, Vector2Int launchPosition)
    {
        return spellDeck.spells[index].CanBeLaunchedAt(this, launchPosition);
    }

    public void AddEtat(Etat etat, int updateValue)
    {
        if (etats.ContainsKey(etat))
        {
            etats[etat] = etats[etat] + updateValue;
        }
        else
        {
            etats.Add(etat, updateValue);
        }
    }

    public void NextTurn()
    {
        statsCurrent.PA = statsInit.PA;
        if (spellDeck != null)
            spellDeck.NextTurn();


        MapManager.Instance.ApplyEndTurnEffects(this);  //Apply end turn effect
    }

    public override bool Equals(object obj)
    {
        var item = obj as Character;

        if (item == null)
        {
            return false;
        }

        return position.Equals(item.position); // gameID?
    }

    public override int GetHashCode()
    {
        return position.GetHashCode();
    }
}
