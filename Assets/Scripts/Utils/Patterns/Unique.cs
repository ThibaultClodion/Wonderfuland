using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This Class assure that a Monobehaviour is the only one to run duuring the game.
/// </summary>

public class Unique<T> : MonoBehaviour where T : Component
{
    private static T instance;

    protected virtual void Awake()
    {
        RemoveDuplicates();
    }

    private void RemoveDuplicates()
    {
        if (instance == null)
        {
            instance = this as T;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}