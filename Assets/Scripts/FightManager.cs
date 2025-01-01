using System.Collections;
using System.Linq;
using UnityEngine;


class FightManager : Singleton<FightManager> 
{
    public static Character.CharacterTeam playingTeam = Character.CharacterTeam.RED;
    public static int turn = 0;

    public static SpawnManagement spawnManagement = new SpawnManagement();
    private static Vector2Int[] spawnPositions;

    private void Update()
    {
        if (GameState.actualState == GameState.State.InCombat && PlayerManager.IAController.characters.Count == 0)
        {
            GameManager.Instance.EndCombat();
        }
    }

    public void InstantiateNewMap(Map map)
    {
        PlayerManager.RemoveAllCharacters();
        MapManager.Instance.ChangeMap(map);

        // Spawn Management
        spawnManagement.StartSpawnSelection();
        spawnPositions = SpawnManagement.GetSpawnPositions();

        PlayerManager.InstantiateAllCharacters(MapManager.Instance.currentMap.spawnPositions);
        InstantiateEnnemies(map);

        //StartCoroutine(EndCombatCoroutine());
    }

    public void InstantiateEnnemies(Map map)
    {
        PlayerManager.IAController.Clear();
        PlayerManager.IAController.InstantiateCharacter(map.ennemies.ToList(), Character.CharacterTeam.RED, map.ennemiesPosition.ToList());
    }

    public void EndCurrentTurn()
    {
        foreach (Character character in CharacterManager.GetCharacters())
        {
            if (character.team == playingTeam)
                character.NextTurn();
        }

        if (playingTeam == Character.CharacterTeam.BLUE)
        {
            playingTeam = Character.CharacterTeam.RED;
            for (int i = 0; i < PlayerManager.playerControllers.Count; i++)
            {
                PlayerManager.playerControllers[i].CantPlay();
            }
        }
        else if (playingTeam == Character.CharacterTeam.RED)
        {
            playingTeam = Character.CharacterTeam.BLUE;
            for (int i = 0; i < PlayerManager.playerControllers.Count; i++)
            {
                PlayerManager.playerControllers[i].CanPlay();
            }
        }

        PlayerManager.IAController.Play(playingTeam);
    }
}
