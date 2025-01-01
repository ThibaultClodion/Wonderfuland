using System;
using System.Collections.Generic;
using System.Reflection;
using TMPro;
using UnityEditor;
using UnityEditor.Build.Content;
using UnityEngine;
using UnityEngine.UI;
using Utils;

namespace UI.Utils
{
    // Amélioration possible -> <T> à l'instanciation de la classe
    // permet de donner la référence à la construction
    public class ReflectionPanel : MonoBehaviour
    {
        private const string suffixValueName = "Field";
        private const string suffixLabelName = "Label";
        public const string prefabPath = "UI/Reflection/";

        // Composants associé à des types
        private GameObject intTypeGO;
        private GameObject enumTypeGO;
        private GameObject boolTypeGO;
        private GameObject labelGO;

        public GameObject labelPanelGO;
        public GameObject valuePanelGO;

        private Type type;
        private Action<string> functionOnFieldUpdate;

        private void LoadBasicComponents()
        {
            intTypeGO = Resources.Load(prefabPath + "IntSliderPrinter") as GameObject;
            enumTypeGO = Resources.Load(prefabPath + "EnumDropdown") as GameObject;
            boolTypeGO = Resources.Load(prefabPath + "BoolButton") as GameObject;
            labelGO = Resources.Load(prefabPath + "Label") as GameObject;
        }

        public void Instantiate<T>(Action<string> _functionOnFieldUpdate)
        {
            Instantiate(typeof(T), _functionOnFieldUpdate);
        }

        public void Instantiate(Type _type, Action<string> _functionOnFieldUpdate)
        {
            Clear();
            LoadBasicComponents();
            type = _type;
            functionOnFieldUpdate = _functionOnFieldUpdate;
            Dictionary<string, Type> fields = ReflectionHelper.GetSpellCreatorFieldsInfos(type); // A changer pour que le code reste générique
            foreach (var field in fields)
            {
                if (valuePanelGO != null)
                {
                    GameObject instantiateGO = InstantiateValueGameObjectFromType(field.Key, field.Value);

                    if (instantiateGO != null)
                    {
                        instantiateGO.name = field.Key + suffixValueName;

                        if (labelPanelGO != null && labelGO != null && labelGO.GetComponent<TextMeshProUGUI>() != null)
                        {
                            GameObject instantiateLabelGO = Instantiate(labelGO, labelPanelGO.transform);
                            instantiateLabelGO.GetComponent<TextMeshProUGUI>().text = field.Key;
                            instantiateLabelGO.name = field.Key + suffixLabelName;
                        }
                    }
                }
            }
        }

        public void UpdateField<T>(ref T objectToUpdate, string fieldName) // ou valeur associé à chaque GO enfant
        {
            if (objectToUpdate.GetType() != type)
            {
                Debug.LogError("Cannot update " + typeof(T) + " / base is " + type);
                return;
            }

            FieldInfo field = type.GetField(fieldName);
            foreach (Transform child in valuePanelGO.GetComponentsInChildren<Transform>())
            {
                if (child.name == fieldName + suffixValueName)
                {
                    int valueGO = UnityReflectionHelper.GetValueFromType(child.gameObject, field.FieldType);
                    if (field.FieldType == typeof(bool))
                    {
                        field.SetValue(objectToUpdate, valueGO != 0);
                    }
                    else
                    {
                        field.SetValue(objectToUpdate, valueGO);
                    }
                }
            }
        }

        public void Clear()
        {
            if (valuePanelGO != null)
            {
                foreach (Transform child in valuePanelGO.GetComponentInChildren<Transform>())
                {
                    Destroy(child.gameObject);
                }
            }

            if (labelPanelGO != null)
            {
                foreach (Transform child in labelPanelGO.GetComponentInChildren<Transform>())
                {
                    Destroy(child.gameObject);
                }
            }
        }

        public void UpdateValues<T>(ref T objectToLoad)
        {
            //if (typeof(T) != type)
            //{
            //    Debug.LogError("Cannot update " + typeof(T) + " / base is " + type);
            //    return;
            //}

            if (valuePanelGO != null)
            {
                foreach (Transform child in valuePanelGO.GetComponentsInChildren<Transform>())
                {
                    string fieldName = child.name.Replace(suffixValueName, "");
                    FieldInfo field = type.GetField(fieldName);

                    if (field != null)
                    {
                        if(field.FieldType == typeof(bool))
                        {
                            if(child.GetComponent<BooleanButton>() != null)
                                child.GetComponent<BooleanButton>().currentValue = (bool)field.GetValue(objectToLoad);
                        }
                        else
                        {
                            int valueField = (int)field.GetValue(objectToLoad);
                            if (child.GetComponent<Slider>() != null)
                            {
                                child.GetComponent<Slider>().value = valueField;
                            }
                            else if (child.GetComponent<TMP_Dropdown>() != null)
                            {
                                child.GetComponent<TMP_Dropdown>().value = valueField;
                            }
                        }
                    }
                }
            }
        }

        private GameObject InstantiateValueGameObjectFromType(string name, Type _type)
        {
            GameObject instantiateGO = null;
            if (_type == typeof(int))
            {
                instantiateGO = Instantiate(intTypeGO, valuePanelGO.transform);
                Slider slider = instantiateGO.GetComponent<Slider>();

                if (slider != null)
                {
                    UnityReflectionHelper.SetSliderLimits(slider, type, name);
                    if (functionOnFieldUpdate != null)
                        slider.onValueChanged.AddListener(delegate { functionOnFieldUpdate(name); });
                }
            }
            else if (_type.BaseType == typeof(Enum))
            {
                instantiateGO = Instantiate(enumTypeGO, valuePanelGO.transform);
                TMP_Dropdown dropdownComponent = instantiateGO.GetComponent<TMP_Dropdown>();

                if (dropdownComponent != null)
                {
                    UnityReflectionHelper.FillDropdownFromEnum(dropdownComponent, _type);
                    if (functionOnFieldUpdate != null)
                        dropdownComponent.onValueChanged.AddListener(delegate { functionOnFieldUpdate(name); });
                }
            }
            else if (_type == typeof(bool))
            {
                instantiateGO = Instantiate(boolTypeGO, valuePanelGO.transform);
                Button buttonComponent = instantiateGO.GetComponent<Button>();

                if (buttonComponent != null)
                {
                    if (functionOnFieldUpdate != null)
                        buttonComponent.onClick.AddListener(delegate { functionOnFieldUpdate(name); });
                }
            }
            return instantiateGO;
        }
    }
}
