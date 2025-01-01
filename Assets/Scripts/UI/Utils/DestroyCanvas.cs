using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyCanvas : MonoBehaviour
{
    public enum AfterDestructionState
    {
        isChoosingSpawnPosition,
        isInCombat,
        isInExploration,
        isChoosingAZone

    }

    [SerializeField] private AfterDestructionState futureState;

    public void SelfDestroy()
    {
        if(futureState == AfterDestructionState.isChoosingSpawnPosition) 
        {
            GameState.IsChoosingSpawnPosition();
        }
        else if (futureState == AfterDestructionState.isInCombat)
        {
            GameState.IsInCombat();
        }
        else if (futureState == AfterDestructionState.isInExploration)
        {
            GameState.IsInExploration();
        }
        else if (futureState == AfterDestructionState.isChoosingAZone)
        {
            GameState.IsChoosingAZone();
        }

        Destroy(gameObject);
    }

}
