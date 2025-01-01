using SpellCreator;
using System;
using System.Collections.Generic;
using UnityEngine;

class CellEffect
{
    private Effect effect;
    private Character launcher;
    [NonSerialized] public CellEffectGO effectGameObject;

    //Effect Duration
    private int turnDuration;
    private int nbTriggerMax;

    //Effect Proc Type
    [NonSerialized] public EffectProcTime procTime;

    //Sub and Primary Effect
    private List<CellEffect> childrenEffects;
    private CellEffect parentEffect;
    private Cell linkedCell;

    #region Initialization

    public void Initialize(Character launcher, Effect effect, CellEffect parent, Cell linkedCell)
    {
        this.launcher = launcher;
        this.effect = effect;
        this.parentEffect = parent;
        this.linkedCell = linkedCell;
        this.childrenEffects = new List<CellEffect>();
        procTime = effect.procTime;
        turnDuration = effect.duration;
        nbTriggerMax = int.MaxValue;   //Must be infinite for only duration effect
    }

    public void SetChildrenEffects(List<CellEffect> childrenEffects)
    {
        this.childrenEffects = childrenEffects;
    }
    #endregion

    public void ApplyEffect(Character target)
    {
        // Apply effect to the target.
        //Il faut ajouter le launcher pour cela.
        //effect.ApplyOnTarget(launcher, target);

        UpdateTriggerMax();
    }

    #region UpdateProcTime

    public void Reset()
    {
        effectGameObject.Deactivate();
    }

    public void RemoveEffect()
    {
        //Destroy childrenEffects
        if(childrenEffects.Count > 0)
        {
            foreach(CellEffect child in childrenEffects) 
            {
                child.RemoveEffect();
            }
        }

        //Remove the effect and destroy his GO
        linkedCell.RemoveEffect(this);
        effectGameObject.Deactivate();
    }

    public void UpdateDuration()
    {
        //If it's a main effect
        if(parentEffect == null)
        {
            turnDuration--;

            if (turnDuration <= 0)
            {
                RemoveEffect();
            }
        }
        else
        {
            parentEffect.UpdateDuration();
        }
    }

    public void UpdateTriggerMax()
    {
        //If it's a main effect
        if(parentEffect == null)
        {
            nbTriggerMax--;

            if (nbTriggerMax <= 0)
            {
                RemoveEffect();
            }
        }
        else
        {
            parentEffect.UpdateTriggerMax();
        }
    }

    #endregion
}
