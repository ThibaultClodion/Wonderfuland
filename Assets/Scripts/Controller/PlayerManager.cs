using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Linq;
using Controller;

class PlayerManager : Singleton<PlayerManager>
{

    //Player Controllers Data
    public static List<PlayerController> playerControllers;
    public static IAController IAController;
    public PlayerInputManager playerInputManager;

    //Zone map
    public GameObject zoneSelectionCanvas;

    //Canvas
    private GameObject currentMainCanvas;
    public GameObject mainMenuCanvas;
    [NonSerialized] public bool mainCanvasIsSelected = false;

    private new void Awake()
    {
        base.Awake();
    }

    private void OnEnable()
    {
        playerControllers = new List<PlayerController>();
    }

    public void OnPlayerJoined(PlayerInput playerInput)
    {
        PlayerController playerController = playerInput.GetComponent<PlayerController>();
        playerControllers.Add(playerController);
        playerController.ResetCommands();

        if (!mainCanvasIsSelected) 
        {
            playerController.eventSystem.SetSelectedGameObject(currentMainCanvas.GetComponentInChildren<Button>().gameObject);
            mainCanvasIsSelected = true;
        }
    }

    public void SetMainCanvas(GameObject mainCanvas)
    {
        currentMainCanvas = Instantiate(mainCanvas);

        if (playerControllers.Count == 0) return; 
        
        playerControllers[0].eventSystem.SetSelectedGameObject(currentMainCanvas.GetComponentInChildren<Button>().gameObject);
        mainCanvasIsSelected = true;
    }

    public void SetMainMenu()
    {
        SetMainCanvas(mainMenuCanvas);
    }

    public void ResetMovements()
    {
        foreach (PlayerController playerController in playerControllers)
        {
            if(playerController.GetCurrentCharacterRB() != null) 
            {
                playerController.GetCurrentCharacterRB().velocity = Vector3.zero;
            }
        }
    }

    public GameObject CreateAZoneSelectionCanvas()
    {
        return Instantiate(zoneSelectionCanvas);
    }

    public static void InstantiateAllCharacters(Vector2Int[] spawnPositions)
    {
        for (int i = 0, j = 0; i < playerControllers.Count; i++)
        {
            if(j >= spawnPositions.Length)
            {
                Debug.LogError("Not enough space to load all characters");
            }

            playerControllers[i].InstantiateCharacters(spawnPositions.ToList().Skip(j).ToList());
            j += playerControllers[i].characters.Count;
        }
    }

    public static void InstantiateOneCharacterPerPlayer(Vector2Int[] spawnPositions)
    {
        for (int i = 0; i < playerControllers.Count; i++)
        {
            playerControllers[i].InstantiateCurrentCharacter(spawnPositions[i]);
        }
    }

    public static void RemoveAllCharacters()
    {
        for (int i = 0; i < playerControllers.Count; i++)
        {
            playerControllers[i].Clear();
        }
        CharacterManager.Clear();
    }
}