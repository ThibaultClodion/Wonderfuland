using System.Collections;
using System.Linq;
using UnityEngine;
using Controller;

class GameManager : Singleton<GameManager>
{
    //Main Menu Canvas
    [SerializeField] public GameObject mainMenuCanvas;

    private new void Awake()
    {
        base.Awake();
    }

    private void Start()
    {
        GameObject IAControllerGO = GameObject.Find("IAController");
        if (IAControllerGO == null)
        {
            Debug.Log("IAController not found");
            return;
        }
        PlayerManager.IAController = IAControllerGO.GetComponent<IAController>();
    }

    public void InstantiateExploration(Vector2Int[] spawnPositions)
    {
        GameState.IsInExploration();

        PlayerManager.RemoveAllCharacters();
        PlayerManager.InstantiateOneCharacterPerPlayer(spawnPositions);
    }

    IEnumerator EndCombatCoroutine()
    {
        yield return new WaitForSeconds(5);
        EndCombat();
    }

    public void EndCombat()
    {
        //Disable all visualisation of combat is equal to tell the players that they can't play.
        for (int i = 0; i < PlayerManager.playerControllers.Count; i++)
        {
            PlayerManager.playerControllers[i].CantPlay();
        }

        InstantiateExploration(MapManager.Instance.currentMap.endCombatPositions);
        MapManager.Instance.EndCombat();
    }

    public static void EndAttraction()
    {
        EffectPool.Clear();
        UnityEngine.SceneManagement.SceneManager.LoadScene(SceneManager.Instance.GetCurrentZone().scene.name);
    }
}
