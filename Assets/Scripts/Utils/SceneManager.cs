using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

class SceneManager : Singleton<SceneManager>
{
    [Header("Zones")]
    [SerializeField] private List<Zone> zones;
    [Header("Menus")]
    [SerializeField] private SceneAsset mainMenu;

    private new void Awake()
    {
        base.Awake();
    }
    private void OnEnable()
    {
        UnityEngine.SceneManagement.SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        UnityEngine.SceneManagement.SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (IsAZone(scene))
        {
            GameManager.Instance.InstantiateExploration(GetAssociatedZone(scene).spawnPositions);
        }
        else if (IsAnAttraction(scene))
        {
            GameManager.Instance.InstantiateExploration(GetAssociatedAttraction(scene).spawnPositions);
        }
        else if (IsMainMenu(scene))
        {
            PlayerManager.Instance.playerInputManager.EnableJoining();
            PlayerManager.Instance.SetMainMenu();
        }

        //Players can't join when the game is launch
        if(!IsMainMenu(scene))
        {
            PlayerManager.Instance.playerInputManager.DisableJoining();
        }
    }

    public bool IsMainMenu(Scene scene)
    {
        return scene.name == mainMenu.name;
    }

    public bool IsAnAttraction(Scene scene)
    {
        return zones.Exists(x => x.attractionDatas.Exists(x => x.scene.name == scene.name));
    }

    public bool IsAZone(Scene scene)
    {
        return zones.Exists(x => x.scene.name == scene.name);
    }

    public void LoadAZone(Zone zone)
    {
        SaveCurrentZone(zone);
        UnityEngine.SceneManagement.SceneManager.LoadScene(zone.scene.name);
    }

    private void SaveCurrentZone(Zone zone)
    {
        PlayerPrefs.SetString("currentZone", zone.scene.name);
    }

    public Zone GetCurrentZone()
    {
        Zone currentZone = zones.Find(x => PlayerPrefs.GetString("currentZone") == x.scene.name);

        if(currentZone == null)
        {
            return zones[0];
        }

        return currentZone;
    }

    public void LoadPreviousZone()
    {
        LoadAZone(GetCurrentZone());
    }

    public Zone GetAssociatedZone(Scene scene)
    {
        return zones.Find(x => x.scene.name == scene.name);
    }

    public Zone.AttractionData GetAssociatedAttraction(Scene scene)
    {
        foreach (Zone zone in zones)
        {
            foreach (Zone.AttractionData attraction in zone.attractionDatas)
            {
                if (attraction.scene.name == scene.name)
                {
                    return attraction;
                }
            }
        }

        return null;
    }
}
