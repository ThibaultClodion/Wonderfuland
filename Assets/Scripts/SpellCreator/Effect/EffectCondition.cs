using System;
using System.Collections.Generic;

namespace SpellCreator
{
    public interface ICondition
    {
        public bool Result();
    }

    // etat>3||(stats.Life<50%&&tour%2)||stats.Life<10%&&(tour>4||etat>5)
    // random>80% -> Réfélchir au random
    class EffectCondition : ICondition
    {
        public enum SeparatorType
        {
            AND,
            OR
        }

        public List<ICondition> effectConditions { private set; get; } = new List<ICondition>();
        public List<SeparatorType> separatorTypes { private set; get; } = new List<SeparatorType>();
        private string condition; // pas utile je le stocke pour mes tests 


        public EffectCondition(string _condition)
        {
            condition = _condition;
            ParseCondition();
        }

        public bool Result()
        {
            bool result = false;
            int separatorIndex = 0;
            int i = 0;
            foreach (ICondition effectCondition in effectConditions)
            {
                if (i == 0)
                {
                    result = effectCondition.Result();
                }
                else
                {
                    if (separatorTypes[separatorIndex] == SeparatorType.OR)
                        result = result || effectCondition.Result();
                    else
                        result = result && effectCondition.Result();
                    separatorIndex++;
                }
                i++;
            }
            return result;
        }

        // Instantiate effectConditions and separatorTypes from condition 
        private void ParseCondition()
        {
            int currentPosition = 0;
            while (currentPosition < condition.Length) // Attention si chaine trop longue
            {
                int parentheseIndex = condition.IndexOf("(", currentPosition);
                int orIndex = condition.IndexOf("||", currentPosition);
                int andIndex = condition.IndexOf("&&", currentPosition);

                bool haveSeparator = orIndex != -1 || andIndex != -1;
                if (!haveSeparator && parentheseIndex == -1)
                {
                    effectConditions.Add(new EffectComparaison(condition.Substring(currentPosition, condition.Length - currentPosition)));
                    break;
                }

                int minSeparatorIndex = GetFirstIndexOfString(andIndex, orIndex);
                if (haveSeparator && (minSeparatorIndex < parentheseIndex || parentheseIndex == -1))
                {
                    effectConditions.Add(new EffectComparaison(condition.Substring(currentPosition, minSeparatorIndex - currentPosition)));
                    currentPosition = minSeparatorIndex;
                    separatorTypes.Add(GetSeparatorType(condition.Substring(minSeparatorIndex, 2)));
                    currentPosition += 2; // + "&&"
                }
                else
                {
                    int endParentheseIndex = condition.IndexOf(")", parentheseIndex);
                    effectConditions.Add(new EffectCondition(condition.Substring(parentheseIndex + 1, endParentheseIndex - parentheseIndex - 1)));
                    currentPosition = endParentheseIndex + 1;

                    orIndex = condition.IndexOf("||", currentPosition);
                    andIndex = condition.IndexOf("&&", currentPosition);
                    haveSeparator = orIndex != -1 || andIndex != -1;
                    if (haveSeparator)
                    {
                        minSeparatorIndex = GetFirstIndexOfString(andIndex, orIndex);
                        separatorTypes.Add(GetSeparatorType(condition.Substring(minSeparatorIndex, 2)));
                        currentPosition += 2;
                    }
                }
            }
        }

        private static SeparatorType GetSeparatorType(string separator)
        {
            if (separator == "&&")
            {
                return SeparatorType.AND;
            }
            else
            {
                return SeparatorType.OR;
            }
        }

        private static int GetFirstIndexOfString(int firstIndex, int secondIndex)
        {
            if (firstIndex == -1)
                return secondIndex;
            else if (secondIndex == -1)
                return firstIndex;
            else
                return Math.Min(firstIndex, secondIndex);
        }
    }

    class EffectComparaison : ICondition
    {
        public enum ComparaisonType
        {
            SUPERIOR,
            SUPERIOR_OR_EQUAL,
            INFERIOR,
            INFERIOR_OR_EQUAL,
            EQUAL,
            DIFFERENT,
            IS_MULTIPLE_OF,
            IS_NOT_MULTIPLE_OF
        }

        public enum ComparaisonValueType
        {
            RANDOM, //toujours entre 0 et 100% // créer une variable utilisable pour garder son tirage
            ETAT,
            STAT,
            USE_COUNT, // Effet qui change selon le nombre d'utilisations
            TARGET_TYPE, // FREE, WALL ...
            TURN,
            VALUE,
            PERCENT
        }

        public ComparaisonType comparaison;
        private List<string> comparaisonCodes = new List<string>{
            ">",
            ">=",
            "<",
            "<=",
            "==",
            "!=",
            "%=",
            "%!"
        };
        public int compareValue1;
        public ComparaisonValueType compareType1;
        public int compareValue2;
        public ComparaisonValueType compareType2;
        private string condition; // pas utile je le stocke pour mes tests 

        //target.etat.fureur > 10 / all.stat.Life < 50% / self.stat.PA < turn / rand1 < rand2
        public EffectComparaison(string _comparaison)
        {
            condition = _comparaison;
            ParseComparaison();
        }

        public bool Result()
        {
            //compareValue1 = GetCompareValue()
            switch (comparaison)
            {
                case ComparaisonType.SUPERIOR:
                    return compareValue1 > compareValue2;
                case ComparaisonType.SUPERIOR_OR_EQUAL:
                    return compareValue1 >= compareValue2;
                case ComparaisonType.INFERIOR:
                    return compareValue1 < compareValue2;
                case ComparaisonType.INFERIOR_OR_EQUAL:
                    return compareValue1 <= compareValue2;
                case ComparaisonType.EQUAL:
                    return compareValue1 == compareValue2;
                case ComparaisonType.DIFFERENT:
                    return compareValue1 != compareValue2;
                case ComparaisonType.IS_MULTIPLE_OF:
                    return compareValue1 % compareValue2 == 0;
                case ComparaisonType.IS_NOT_MULTIPLE_OF:
                    return compareValue1 % compareValue2 != 0;
            }
            return false;
        }

        private void ParseComparaison()
        {
            int comparaisonIndex = -1;
            foreach(string comparaisonCode in comparaisonCodes)
            {
                comparaisonIndex = condition.IndexOf(comparaisonCode);
                if(comparaisonIndex != -1)
                {
                    comparaison = GetCompareType(comparaisonCode);
                    break;
                }
            }

            if (comparaisonIndex == -1)
                throw new Exception("No comparaison in effectCondition");

            string compareCode1 = condition.Substring(0, comparaisonIndex);
            string compareCode2 = condition.Substring(comparaisonIndex);
            compareType1 = GetComparaisonValueType(compareCode1);
            compareType2 = GetComparaisonValueType(compareCode2);
        }

        //TODO : Derniere Etape
        private int GetCompareValue(Character character, int turn)
        {
            if(compareType1 == ComparaisonValueType.RANDOM)
            {

            }
            else if(compareType1 == ComparaisonValueType.ETAT)
            {
                
            }
            return 1;
        }

        private static ComparaisonType GetCompareType(string _comparaison)
        {
            switch(_comparaison)
            {
                case ">":
                    return ComparaisonType.SUPERIOR;
                case ">=":
                    return ComparaisonType.SUPERIOR_OR_EQUAL;
                case "<":
                    return ComparaisonType.INFERIOR;
                case "<=":
                    return ComparaisonType.INFERIOR_OR_EQUAL;
                case "==":
                    return ComparaisonType.EQUAL;
                case "!=":
                    return ComparaisonType.DIFFERENT;
                case "%=":
                    return ComparaisonType.IS_MULTIPLE_OF;
                case "%!":
                    return ComparaisonType.IS_NOT_MULTIPLE_OF;
            }
            throw new Exception("Bad string format for spell effect condition");
        }

        private static ComparaisonValueType GetComparaisonValueType(string code)
        {
            if(int.TryParse(code, out int compareValue1))
            {
                return ComparaisonValueType.VALUE;
            }
            else if(code.Contains("%"))
            {
                return ComparaisonValueType.PERCENT;
            }
            else if(code.Equals("turn"))
            {
                return ComparaisonValueType.TURN;
            }
            else if(code.Contains("etat"))
            {
                return ComparaisonValueType.ETAT;
            }
            else if(code.Contains("stat"))
            {
                return ComparaisonValueType.STAT;
            }
            else if(code.Contains("count"))
            {
                return ComparaisonValueType.USE_COUNT;
            }
            else if (code.Contains("team"))
            {
                return ComparaisonValueType.TARGET_TYPE;
            }
            else if (code.Contains("random"))
            {
                return ComparaisonValueType.RANDOM;
            }

            throw new InvalidCastException("Can't parse " + code + " as ComparaisonValueType");
        }
    }
}
