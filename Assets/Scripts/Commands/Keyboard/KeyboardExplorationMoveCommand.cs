using UnityEngine;
using UnityEngine.InputSystem;
using Controller;

class KeyboardExplorationMoveCommand : BaseCommand
{
    public KeyboardExplorationMoveCommand(PlayerInputs keyboardInput, PlayerInputs gamepadInput, PlayerController playerController) :
        base(keyboardInput, gamepadInput, playerController)
    {
        GiveCommandToPlayer(this); }

    public override void Execute(InputValue value)
    {
        playerController.movement = new Vector3(value.Get<Vector2>().x, 0, value.Get<Vector2>().y);
    }
}
