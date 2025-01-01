using Newtonsoft.Json;
using System;
using Utils;

namespace SpellCreator
{
    class Stats
    {
        [ReflectionLimit(4, 10)] public int PA;
        [ReflectionLimit(50, 100)] public int Life;
        [JsonIgnore] public int ResistancePercent;
        [JsonIgnore] public int Shield;
        [JsonIgnore] public int DamagePercent;

        // Réfléchir aux stats
        public enum StatEnum
        {
            PA,
            Life,
            ResistancePercent,
            DamagePercent,
            Shield
        }

        public Stats()
        {

        }

        private ref int GetStatValueFromEnum(StatEnum statEnum)
        {
            if (statEnum == StatEnum.PA)
                return ref PA;
            else if (statEnum == StatEnum.Life)
                return ref Life;
            else if(statEnum == StatEnum.ResistancePercent)
                return ref ResistancePercent;
            else if(statEnum == StatEnum.DamagePercent)
                return ref DamagePercent;
            else if(statEnum == StatEnum.Shield)
                return ref Shield;

            throw new Exception("statEnum doesn't implemented");
        }

        public void LoseStat(StatEnum stat, int value)
        {
            GetStatValueFromEnum(stat) -= value;
        }

        public void GainStat(StatEnum stat, int value)
        {
            GetStatValueFromEnum(stat) += value;
        }

        public int GetStatValue(StatEnum stat)
        {
            return GetStatValueFromEnum(stat);
        }

        public Stats Clone()
        {
            Stats stats = new Stats();
            stats.PA = PA;
            stats.Life = Life;
            stats.ResistancePercent = ResistancePercent;    
            stats.DamagePercent = DamagePercent;
            stats.Shield = Shield;
            return stats;
        }
    }
}