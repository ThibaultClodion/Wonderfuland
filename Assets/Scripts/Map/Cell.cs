using SpellCreator;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class Cell
{
    private CellType type;
    private List<CellEffect> cellEffects;

    public enum CellType
    {
        Ground,
        Hole,
        Wall,
        Empty
    }

    public enum CellOccupation
    {
        FREE,
        CHARACTER_ON_IT
    }

    public Cell()
    {
        type = CellType.Empty;
        cellEffects = new List<CellEffect>();
    }

    public static CellOccupation GetCellOccupation(Vector2Int position)
    {
        return FightData.GetCharacterByPosition(position) == null ? CellOccupation.FREE : CellOccupation.CHARACTER_ON_IT;
    }

    #region TypeManagement
    public void SetType(CellType type)
    {
        this.type = type;
    }

    public CellType GetCellType()
    {
        return type;
    }
    #endregion

    #region EffectsManagement

    public void AddCellEffect(CellEffect effect)
    {
        cellEffects.Add(effect);
    }

    public void RemoveEffect(CellEffect effect)
    {
        cellEffects.Remove(effect);
    }

    public void ResetEffects()
    {
        CellEffect[] effectCopy = cellEffects.ToArray();

        foreach (CellEffect effect in effectCopy)
        {
            effect.Reset();
        }

        cellEffects = new List<CellEffect>();
    }

    public void ApplyWalkOnEffects(Character character)
    {
        List<CellEffect> copyEffects = new List<CellEffect>(cellEffects); //The copy is to not trying access destroyed effects

        foreach (CellEffect effect in copyEffects)
        {
            if (effect.procTime == EffectProcTime.NOW)
            {
                effect.ApplyEffect(character);
            }
        }
    }

    public void ApplyEndTurnEffects(Character character)
    {
        List<CellEffect> copyEffects = new List<CellEffect>(cellEffects);   //The copy is to not trying access destroyed effects

        foreach (CellEffect effect in copyEffects)
        {
            if (effect.procTime == EffectProcTime.END_OF_SELF_TURN)
            {
                effect.ApplyEffect(character);
            }
        }
    }

    public void ApplyStartTurnEffects(Character character)
    {
        List<CellEffect> copyEffects = new List<CellEffect>(cellEffects);   //The copy is to not trying access destroyed effects

        foreach (CellEffect effect in copyEffects)
        {
            if (effect.procTime == EffectProcTime.START_OF_SELF_TURN)
            {
                effect.ApplyEffect(character);
            }
        }
    }

    public void EndTurn()
    {
        foreach (CellEffect effect in cellEffects)
        {
            effect.UpdateDuration();
        }
    }
    #endregion
}
