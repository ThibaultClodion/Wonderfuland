using SpellCreator;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

class EffectPool : MonoBehaviour
{
    // stack-based ObjectPool available with Unity 2021 and above
    private static IObjectPool<CellEffectGO> effectGOPool;
    [SerializeField] private CellEffectGO effectPrefab;
    private GameObject effectPoolContainerGO;

    // throw an exception if we try to return an existing item, already in the pool
    private bool collectionCheck = true;

    // extra options to control the pool capacity and maximum size
    [SerializeField] private int defaultCapacity = 20;
    [SerializeField] private int maxSize = 100;

    #region PoolSystem
    private void Awake()
    {
        
        effectGOPool = new ObjectPool<CellEffectGO>(CreatePoolEffect,
            OnGetFromPool, OnReleaseToPool, OnDestroyPooledObject,
            collectionCheck, defaultCapacity, maxSize);
    }

    // invoked when creating an item to populate the object pool
    private CellEffectGO CreatePoolEffect()
    {
        if (effectPoolContainerGO == null)
        {
            effectPoolContainerGO = new GameObject("EffectPool");
        }
        CellEffectGO effectInstance = Instantiate(effectPrefab, effectPoolContainerGO.transform);
        effectInstance.ObjectPool = effectGOPool;
        return effectInstance;
    }

    // invoked when returning an item to the object pool
    private void OnReleaseToPool(CellEffectGO pooledObject)
    {
        pooledObject.gameObject.SetActive(false);
    }

    // invoked when retrieving the next item from the object pool
    private void OnGetFromPool(CellEffectGO pooledObject)
    {
        pooledObject.gameObject.SetActive(true);
    }

    // invoked when we exceed the maximum number of pooled items (i.e. destroy the pooled object)
    private void OnDestroyPooledObject(CellEffectGO pooledObject)
    {
        Destroy(pooledObject.gameObject);
    }
    #endregion

    public static void Clear()
    {
        effectGOPool.Clear();
    }

    private static CellEffectGO CreateEffect(Vector2Int position)
    {
        CellEffectGO effectGO = effectGOPool.Get();

        if (effectGO == null) { return null; }

        MapManager.Instance.PutGameObjectOnCurrentMap(effectGO.gameObject, position);

        return effectGO;
    }

    public static CellEffect SetEffects(List<Vector2Int> positions, Color color)
    {
        //Define the first cell as the main one
        CellEffect mainEffect = SetEffect(positions[0], null, color);

        //Initialize the others effects
        List<CellEffect> childEffects = new List<CellEffect>();

        positions.Remove(positions[0]); //To not have duplication of the first position
        foreach(Vector2Int position in positions)
        {
            CellEffect childEffect = SetEffect(position, mainEffect, color);
            childEffects.Add(childEffect);
        }

        //Set Children Effects to the main one
        mainEffect.SetChildrenEffects(childEffects);

        return mainEffect;
    }

    private static CellEffect SetEffect(Vector2Int position, CellEffect parent, Color color)
    {
        Cell cell = MapManager.Instance.GetCell(position);  //Find the cell

        CellEffect cellEffect = new CellEffect();
        cellEffect.Initialize(null, new Effect(), parent, cell);    //Initialize null effect not launched by player

        //Pool Management
        CellEffectGO gameObjectEffect = CreateEffect(position);
        gameObjectEffect.GetComponent<Renderer>().material.color = color;
        cellEffect.effectGameObject = gameObjectEffect;

        cell.AddCellEffect(cellEffect); //Add the effect to the cell

        return cellEffect;
    }
}