using Controller;
using UnityEngine;
using UnityEngine.InputSystem;

class CinematicMoveCommand : BaseCommand
{
    public CinematicMoveCommand(PlayerInputs keyboardInput, PlayerInputs gamepadInput, PlayerController playerController) :
        base(keyboardInput, gamepadInput, playerController)
    { GiveCommandToPlayer(this); }

    public override void Execute(InputValue value)
    {
        if (CinematicManager.instance == null) return;

        CinematicManager.instance.cinematicCamera.Rotate(value.Get<Vector2>());
    }
}