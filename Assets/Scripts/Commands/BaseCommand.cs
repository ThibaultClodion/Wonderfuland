using Controller;
using UnityEngine.InputSystem;

abstract class BaseCommand
{
    protected PlayerController playerController;
    private PlayerInputs input;

    protected BaseCommand(PlayerInputs keyboardInput, PlayerInputs gamepadInput, PlayerController playerController)
    {
        this.playerController = playerController;

        input = keyboardInput;

        if (playerController.playerInput.devices[0].ToString().Contains("Gamepad")) { input = gamepadInput;}
    }

    public void GiveCommandToPlayer(BaseCommand command)
    {
        switch (input)
        {
            case PlayerInputs.LeftStickOrWASD:
                playerController.leftStickOrWASDCommand = command;
                break;
            case PlayerInputs.RightStickOrMouse:
                playerController.rightStickOrMouseCommand = command;
                break;
            case PlayerInputs.AOrLeftClick:
                playerController.aOrLeftClickCommand = command;
                break;
            case PlayerInputs.XOrSpace:
                playerController.xOrSpaceommand = command;
                break;
            case PlayerInputs.YOrRightClick:
                playerController.yOrRightClickCommand = command;
                break;
            case PlayerInputs.LeftTriggerOrArrow:
                playerController.leftTriggerOrArrowCommand = command;
                break;
            case PlayerInputs.RightTriggerOrArrow:
                playerController.rightTriggerOrArrowCommand = command;
                break;
            case PlayerInputs.SelectOrTab:
                playerController.selectOrTabCommand = command;
                break;
            case PlayerInputs.StartOrEchap:
                playerController.startOrEchapCommand = command;
                break;
            case PlayerInputs.BOrE:
                playerController.bOrECommand = command;
                break;
            default:
                break;
        }
    }

    public virtual void Execute(InputValue value) { return; }
}