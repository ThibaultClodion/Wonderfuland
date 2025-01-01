using SpellCreator;
using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using Utils;
using UI.Utils;

namespace UI
{
    class SpellCreatorUI : MonoBehaviour
    {
        public ReflectionPanel spellPanelGO;
        public ReflectionPanel effectPanelGO;
        public SpellBar spellBarGO;
        public TMP_Dropdown effectUsedDropdown;
        public TMP_Dropdown subEffectDropdown;

        [HideInInspector] public Effect currentEffect;
        [HideInInspector] public Spell currentSpell;
        private Character currentCharacter;

        public void Awake()
        {
            spellPanelGO.Instantiate<Spell>(UpdateSpellFromCharacter);

            UnityReflectionHelper.FillDropdownFromEnum(subEffectDropdown, typeof(subEffectType));
            effectPanelGO.Instantiate(GetTypeFromSubEffect((subEffectType)subEffectDropdown.value), UpdateEffectFromSpell);

            // Renseigner subEffectDropdown à l'ajout de l'effet si on veut plus de simplicité
            subEffectDropdown.onValueChanged.AddListener(delegate
            {
                Type subEffect = GetTypeFromSubEffect((subEffectType)subEffectDropdown.value);
                effectPanelGO.Instantiate(subEffect, UpdateEffectFromSpell);
                if(currentEffect == null || currentEffect.GetType() != subEffect)
                {
                    currentSpell.effects[effectUsedDropdown.value] = GetSubEffectFromType((subEffectType)subEffectDropdown.value);
                    currentEffect = currentSpell.effects[effectUsedDropdown.value];
                }
                effectPanelGO.UpdateValues(ref currentEffect);
            });

            effectUsedDropdown.onValueChanged.AddListener(delegate
            {
                UseSelectedEffect(effectUsedDropdown.value);
            });
        }

        public void Initialize(ref Character character)
        {
            currentCharacter = character;
            
            spellBarGO.Initialize(UseSelectedSpell, character);
            spellBarGO.UnselectSpell();
            spellBarGO.UpdateSelectedSpellColor(0);
        }

        // Events
        // On generated spell fields update
        public void UpdateSpellFromCharacter(string fieldName)
        {
            spellPanelGO.UpdateField(ref currentSpell, fieldName);
        }

        // On generated effect fields update
        public void UpdateEffectFromSpell(string fieldName)
        {
            effectPanelGO.UpdateField(ref currentEffect, fieldName);
        }

        // Add Effect button
        public void AddEffect()
        {
            currentSpell.effects.Add(GetSubEffectFromType((subEffectType)subEffectDropdown.value));
            List<TMP_Dropdown.OptionData> newOptions = new List<TMP_Dropdown.OptionData>
            {
                new TMP_Dropdown.OptionData("Effect " + currentSpell.effects.Count.ToString())
            };
            effectUsedDropdown.AddOptions(newOptions);
            effectUsedDropdown.value = currentSpell.effects.Count - 1;
        }

        // On SpellBar spell click + Call in Initialize 
        public void UseSelectedSpell(int index)
        {
            currentSpell = currentCharacter.spellDeck.deck[index];
            spellPanelGO.UpdateValues(ref currentSpell);

            if (currentSpell.effects.Count == 0)
            {
                currentSpell.effects.Add(GetSubEffectFromType((subEffectType)subEffectDropdown.value));
            }

            effectUsedDropdown.ClearOptions();
            List<TMP_Dropdown.OptionData> newOptions = new List<TMP_Dropdown.OptionData>();
            for(int i = 0; i < currentSpell.effects.Count; i++)
            {
                newOptions.Add(new TMP_Dropdown.OptionData("Effect " + (i + 1).ToString()));
            }
            effectUsedDropdown.AddOptions(newOptions);
            
            if (effectUsedDropdown.value == 0)
                UseSelectedEffect(0);
            effectUsedDropdown.value = 0;
        }

        // On effectUsedDropdown update
        public void UseSelectedEffect(int index)
        {
            currentEffect = currentSpell.effects[index];
            if (GetTypeFromSubEffect((subEffectType)subEffectDropdown.value) == currentEffect.GetType())
            {
                effectPanelGO.UpdateValues(ref currentEffect);
            }
            else
            {
                subEffectDropdown.value = (int)GetSubEffectFromType(currentEffect.GetType());
            }
        }

        #region Gestion des sous classes effects
        public enum subEffectType
        {
            DAMAGE,
            INVOCATION,
            TRANSFORMATION,
            ETAT_APPLY,
            ALTERATION,
            MOVEMENT
        }

        public static Effect GetSubEffectFromType(subEffectType subEffectType) // utile ? / voir pour faire ça mieux
        {
            if (subEffectType == subEffectType.DAMAGE)
            {
                return new Damage();
            }
            else if (subEffectType == subEffectType.INVOCATION)
            {
                return new Invocation();
            }
            else if (subEffectType == subEffectType.TRANSFORMATION)
            {
                return new Transformation();
            }
            else if (subEffectType == subEffectType.ETAT_APPLY)
            {
                return new EtatApply();
            }
            else if (subEffectType == subEffectType.ALTERATION)
            {
                return new Alteration();
            }
            else if (subEffectType == subEffectType.MOVEMENT)
            {
                return new Movement();
            }
            return null;
        }

        public static Type GetTypeFromSubEffect(subEffectType subEffectType) // utile ? / voir pour faire ça mieux
        {
            if (subEffectType == subEffectType.DAMAGE)
            {
                return typeof(Damage);
            }
            else if (subEffectType == subEffectType.INVOCATION)
            {
                return typeof(Invocation);
            }
            else if (subEffectType == subEffectType.TRANSFORMATION)
            {
                return typeof(Transformation);
            }
            else if (subEffectType == subEffectType.ETAT_APPLY)
            {
                return typeof(EtatApply);
            }
            else if (subEffectType == subEffectType.ALTERATION)
            {
                return typeof(Alteration);
            }
            else if (subEffectType == subEffectType.MOVEMENT)
            {
                return typeof(Movement);
            }
            return null;
        }

        public static subEffectType GetSubEffectFromType(Type type)
        {
            if (type == typeof(Damage))
            {
                return subEffectType.DAMAGE;
            }
            else if (type == typeof(Invocation))
            {
                return subEffectType.INVOCATION;
            }
            else if (type == typeof(Transformation))
            {
                return subEffectType.TRANSFORMATION;
            }
            else if (type == typeof(EtatApply))
            {
                return subEffectType.ETAT_APPLY;
            }
            else if (type == typeof(Alteration))
            {
                return subEffectType.ALTERATION;
            }
            else if (type == typeof(Movement))
            {
                return subEffectType.MOVEMENT;
            }
            return subEffectType.DAMAGE;
        }
        #endregion
    }
}
