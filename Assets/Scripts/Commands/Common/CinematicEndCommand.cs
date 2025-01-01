using Controller;
using UnityEngine.InputSystem;

class CinematicEndCommand : BaseCommand
{
    public CinematicEndCommand(PlayerInputs keyboardInput, PlayerInputs gamepadInput, PlayerController playerController) :
        base(keyboardInput, gamepadInput, playerController){ GiveCommandToPlayer(this); }

    public override void Execute(InputValue value)
    {
        CinematicManager.instance.End();
    }
}
