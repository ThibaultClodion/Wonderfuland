using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class CellEffectGO : MonoBehaviour
{
    private IObjectPool<CellEffectGO> objectPool;

    // public property to give the projectile a reference to its ObjectPool
    public IObjectPool<CellEffectGO> ObjectPool { set => objectPool = value; }

    public void Deactivate()
    {
        objectPool.Release(this);
    }
}
