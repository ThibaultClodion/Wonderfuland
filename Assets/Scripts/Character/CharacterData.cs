using System.Collections;
using System.Linq;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu]
public class CharacterData : ScriptableObject
{
    public GameObject appearanceGO;
    //public IAType IA; // enum
    [SerializeField] private Object characterFile;
    [HideInInspector] public string filePath; 
}

[CustomEditor(typeof(CharacterData))]
public class StreamingImageEditor : Editor
{

    SerializedProperty filePath;
    SerializedProperty characterFile;

    void OnEnable()
    {
        filePath = serializedObject.FindProperty("filePath");
        characterFile = serializedObject.FindProperty("characterFile");
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if (characterFile.objectReferenceValue == null)
        {
            filePath.stringValue = string.Empty;
        }
        else
        {
            string path = AssetDatabase.GetAssetPath(characterFile.objectReferenceValue.GetInstanceID());
            filePath.stringValue = path.Split('/').Last();
        }
        serializedObject.ApplyModifiedProperties();
    }
}