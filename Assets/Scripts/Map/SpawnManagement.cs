using SpellCreator;
using System.Linq;
using UnityEngine;
using Controller;

class SpawnManagement
{
    private static CellEffect spawnEffect;
    private static int nbPlayerReady = 0;
    private Color spawnEffectColor = new Color32(65, 193, 250, 160);

    public void StartSpawnSelection()
    {
        GameState.IsChoosingSpawnPosition();

        Vector2Int[] spawnPositions = GetSpawnPositions();

        if (spawnPositions.Length > 0)
        {
            spawnEffect = EffectPool.SetEffects(spawnPositions.ToList(), spawnEffectColor);
        }

        foreach (PlayerController playerController in PlayerManager.playerControllers)
        {
            playerController.hasSpawnSet = false;
        }
    }

    private static void EndSpawnSelection()
    {
        nbPlayerReady = 0;

        if(spawnEffect != null) 
        {
            spawnEffect.RemoveEffect();
        }

        GameState.IsInCombat();
        FightManager.Instance.EndCurrentTurn(); //Starting the fight is the same as ending a turn
    }

    public static Vector2Int[] GetSpawnPositions()
    {
        return MapManager.Instance.currentMap.spawnPositions;
    }

    public bool IsAValidSpawnPosition(Vector2Int position)
    {
        foreach (Vector2Int spawnPosition in MapManager.Instance.currentMap.spawnPositions)
        {
            if (spawnPosition == position && !CharacterManager.HasACharacterOnThisPosition(spawnPosition))
            {
                return true;
            }
        }

        return false;
    }

    public Vector2Int GetGamepadSpawnPosition(Vector2Int movement, Vector2Int initialPosition)
    {
        if (movement.x != 0)
        {
            for (int y = 0; y < MapManager.Instance.GetLength(); y++)
            {
                for (int x = 1; x < MapManager.Instance.GetWidth(); x++)
                {
                    if (IsAValidSpawnPosition(initialPosition + new Vector2Int(movement.x * x, y)))
                    {
                        return initialPosition + new Vector2Int(movement.x * x, y);
                    }
                    else if (IsAValidSpawnPosition(initialPosition + new Vector2Int(movement.x * x, -y)))
                    {
                        return initialPosition + new Vector2Int(movement.x * x, -y);
                    }
                }
            }
        }
        else if (movement.y != 0)
        {
            for (int x = 0; x < MapManager.Instance.GetWidth(); x++)
            {
                for (int y = 1; y < MapManager.Instance.GetLength(); y++)
                {
                    if (IsAValidSpawnPosition(initialPosition + new Vector2Int(x, movement.y * y)))
                    {
                        return initialPosition + new Vector2Int(x, movement.y * y);
                    }
                    else if (IsAValidSpawnPosition(initialPosition + new Vector2Int(-x, movement.y * y)))
                    {
                        return initialPosition + new Vector2Int(-x, movement.y * y);
                    }
                }
            }
        }

        return initialPosition;
    }

    public static void ConfirmSpawnPosition()
    {
        nbPlayerReady++;


        if (nbPlayerReady == PlayerManager.playerControllers.Count)
        {
            EndSpawnSelection();
        }
    }

    public static void UnconfirmSpawnPosition()
    {
        nbPlayerReady--;
    }
}
