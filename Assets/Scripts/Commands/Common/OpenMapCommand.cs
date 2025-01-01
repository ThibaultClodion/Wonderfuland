using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using Controller;

class OpenMapCommand : BaseCommand
{
    private GameObject actualDisplayedCanvas = null;

    public OpenMapCommand(PlayerInputs keyboardInput, PlayerInputs gamepadInput, PlayerController playerController) :
        base(keyboardInput, gamepadInput, playerController){ GiveCommandToPlayer(this); }

    public override void Execute(InputValue value)
    {
        if(SceneManager.Instance.IsAZone(UnityEngine.SceneManagement.SceneManager.GetActiveScene()))
        {
            if (actualDisplayedCanvas == null)
            {
                OpenMap();
            }
            else
            {
                CloseMap();
            }
        }
    }

    public void CloseMap()
    {
        GameState.IsStoppingChoosingAZone();
        GameObject.Destroy(actualDisplayedCanvas);
    }

    private void OpenMap()
    {
        if(GlobalUI.CanCreateAGlobalUI())
        {
            GameState.IsChoosingAZone();

            if (actualDisplayedCanvas == null)
            {
                actualDisplayedCanvas = PlayerManager.Instance.CreateAZoneSelectionCanvas();
                playerController.eventSystem.SetSelectedGameObject(GetActualCanvasButtonGameObject());
            }
        }
    }

    private GameObject GetActualCanvasButtonGameObject()
    {
        return actualDisplayedCanvas.GetComponentInChildren<Button>().gameObject;
    }
}
