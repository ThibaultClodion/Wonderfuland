using Newtonsoft.Json;
using System.Collections.Generic;
using UnityEngine;
using Utils;

namespace SpellCreator
{
    public enum EffectProcTime
    {
        NOW,
        START_OF_SELF_TURN,
        END_OF_SELF_TURN,
        START_OF_TARGET_TURN,
        END_OF_TARGET_TURN,
        SELF_DEATH,
        TARGET_DEATH
    }

    class Effect
    {
        [JsonIgnore]
        private EffectCondition condition;

        [JsonIgnore]
        private int counter;

        public List<Vector2Int> effectZone;
        [ReflectionLimit(0, 10)] public int duration;
        public bool cellEffect;
        public bool persistentCellEffect;

        public string conditionCode;
        public EffectProcTime procTime;

        public Effect()
        {
            effectZone = new List<Vector2Int>();
            //condition = new EffectCondition(conditionCode.Replace(" ", string.Empty));
        }
       
        public bool CanBeTriggered(Stats stats)
        {
            if (condition != null)
                return condition.Result();
            return true;
        }

        public virtual void Apply(Character launcher, Vector2Int position){}
        public virtual void RollbackApply(Character launcher, Vector2Int position) { }
    }
}