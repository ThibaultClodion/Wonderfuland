using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Reflection;
using TMPro;
using UI.Utils;
using UnityEngine;
using UnityEngine.UI;

namespace Utils
{
    public class ReflectionLimit : Attribute
    {
        public int min;
        public int max;

        public ReflectionLimit(int _min, int _max)
        {
            min = _min;
            max = _max;
        }
    }

    public static class UnityReflectionHelper
    {
        public static void FillDropdownFromEnum(TMP_Dropdown dropdown, Type _type)
        {
            if (_type.BaseType == typeof(Enum) && dropdown != null)
            {
                FieldInfo[] infos = _type.GetFields();
                dropdown.options.Clear();
                for (int i = 1; i < infos.Length; i++) // premier indice == _value
                {
                    dropdown.options.Add(new TMP_Dropdown.OptionData(infos[i].Name));
                }
            }
        }

        public static void SetSliderLimits(Slider slider, Type type, string fieldName)
        {
            ReflectionLimit limit = (ReflectionLimit)ReflectionHelper.GetGivenAttribute<ReflectionLimit>(type.GetField(fieldName));
            if (limit != null)
            {
                slider.minValue = limit.min;
                slider.maxValue = limit.max;
                //slider.value = limit.min + (limit.max - limit.min) / 2;
            }
        }

        public static int GetValueFromType(GameObject valueGO, Type _type)
        {
            if (_type.BaseType == typeof(Enum))
            {
                TMP_Dropdown dropdown = valueGO.GetComponent<TMP_Dropdown>();
                if (dropdown == null)
                {
                    Debug.LogError("No dropdown for given type " + _type);
                    return 0;
                }
                return dropdown.value;
            }
            else if (_type == typeof(int))
            {
                Slider slider = valueGO.GetComponent<Slider>();
                if (slider == null)
                {
                    Debug.LogError("No slider for given type " + _type);
                    return 0;
                }
                return (int)slider.value;
            }
            else if (_type == typeof(bool))
            {
                BooleanButton button = valueGO.GetComponent<BooleanButton>();
                if (button == null)
                {
                    Debug.LogError("No BooleanButton for given type " + _type);
                    return 0;
                }
                return button.currentValue ? 1 : 0;
            }

            Debug.LogError("No valueGO for given type " + _type);
            return 0;
        }
    }

    public static class ReflectionHelper
    {
        public static Dictionary<string, Type> GetFieldsInfos(Type type)
        {
            Dictionary<string, Type> fields = new Dictionary<string, Type>();

            FieldInfo[] infos = type.GetFields();
            foreach (FieldInfo info in infos)
            {
                fields.Add(info.Name, info.FieldType);
            }
            return fields;
        }

        public static Dictionary<string, Type> GetSpellCreatorFieldsInfos(Type type)
        {
            Dictionary<string, Type> fields = new Dictionary<string, Type>();

            FieldInfo[] infos = type.GetFields();
            foreach (FieldInfo info in infos)
            {
                if (info.GetCustomAttribute<JsonIgnoreAttribute>() != null)
                    continue;
                if(info.IsPrivate || info.IsStatic) 
                    continue;
                fields.Add(info.Name, info.FieldType);
            }
            return fields;
        }

        public static Attribute GetGivenAttribute<T>(FieldInfo field)
        {
            if(field != null && typeof(T).BaseType == typeof(Attribute))
            {
               return field.GetCustomAttribute(typeof(T));
            }
            return null;
        }
    }
}
