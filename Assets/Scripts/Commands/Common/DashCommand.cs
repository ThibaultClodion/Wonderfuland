using UnityEngine.InputSystem;
using Controller;

class DashCommand : BaseCommand
{
    public DashCommand(PlayerInputs keyboardInput, PlayerInputs gamepadInput, PlayerController playerController) :
        base(keyboardInput, gamepadInput, playerController)
    { GiveCommandToPlayer(this); }

    public override void Execute(InputValue value)
    {
        playerController.Dash();
    }
}
