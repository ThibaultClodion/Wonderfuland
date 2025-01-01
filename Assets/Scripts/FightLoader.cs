using Controller;
using UnityEngine;
using UnityEngine.InputSystem;

class FightLoader : MonoBehaviour
{
    public Map map;
    public GameObject launchCanvas;


    private static SpawnManagement spawnManagement;
    private static Vector2Int[] spawnPositions;

    void Start()
    {
        GameObject IAControllerGO = GameObject.Find("IAController");
        if (IAControllerGO == null)
        {
            Debug.Log("IAController not found");
            return;
        }
        PlayerManager.IAController = IAControllerGO.GetComponent<IAController>();
        PlayerManager.Instance.mainCanvasIsSelected = true;
    }

    public void LaunchFight()
    {
        Destroy(launchCanvas);
       
        MapManager.Instance.ChangeMapFromPrefab(map);
        FightManager.spawnManagement.StartSpawnSelection();

        PlayerManager.InstantiateAllCharacters(SpawnManagement.GetSpawnPositions());
        FightManager.Instance.InstantiateEnnemies(map);
        
        //GameState.IsInCombat();
    }

    // Update is called once per frame
    void Update()
    {

    }
}