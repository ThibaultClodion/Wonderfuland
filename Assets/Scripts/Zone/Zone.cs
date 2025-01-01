using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu]
class Zone : ScriptableObject
{
    [System.Serializable]
    public class AttractionData
    {
        [SerializeField] public SceneAsset scene;
        [SerializeField] public Vector2Int[] spawnPositions;
    }

    [Header("Zone")]
    [SerializeField] public SceneAsset scene;
    [SerializeField] public Vector2Int[] spawnPositions;

    [Header("Attractions")]
    [SerializeField] public List<AttractionData> attractionDatas;
}
