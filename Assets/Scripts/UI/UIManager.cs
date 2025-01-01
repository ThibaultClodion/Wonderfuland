using UnityEngine;
using Controller;

public class UIManager : Singleton<UIManager>
{
    [Header("Fight UI")]
    [SerializeField] private GameObject fightCanvas;


    private GameObject actualDisplayCanvas;

    private new void Awake()
    {
        base.Awake();
    }

    public void RenderFightUI()
    {
        //Instantiate the fight Canvas
        DestroyOldCanvas();
        actualDisplayCanvas = Instantiate(fightCanvas);

        //Instantiate the character UI and give it to the players
        GameObject characterUI = actualDisplayCanvas.transform.Find("CharacterUI").gameObject;
        
        if (PlayerManager.playerControllers.Count > 1)
        {
            Debug.LogWarning("Problème d'UI ?");
        }

        foreach(PlayerController playerController in PlayerManager.playerControllers) // boucle ?
        {
            playerController.SetCharacterUI(characterUI);
        }
    }

    public void RenderSpawnSelectionUI()
    {
        DestroyOldCanvas();
    }

    public void RenderExplorationUI()
    {
        DestroyOldCanvas();
    }

    private void DestroyOldCanvas()
    {
        if(actualDisplayCanvas != null) 
        {
            Destroy(actualDisplayCanvas);
        }
    }
}
