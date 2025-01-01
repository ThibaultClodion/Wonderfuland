using Controller;
using UnityEngine.InputSystem;

class CinematicPauseCommand: BaseCommand
{
    public CinematicPauseCommand(PlayerInputs keyboardInput, PlayerInputs gamepadInput, PlayerController playerController) :
        base(keyboardInput, gamepadInput, playerController)
    { GiveCommandToPlayer(this); }

    public override void Execute(InputValue value)
    {
        CinematicManager.instance.Pause();
    }
}
