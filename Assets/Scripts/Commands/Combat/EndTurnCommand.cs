using UnityEngine.InputSystem;
using Controller;

class EndTurnCommand : BaseCommand
{
    public EndTurnCommand(PlayerInputs keyboardInput, PlayerInputs gamepadInput, PlayerController playerController) :
        base(keyboardInput, gamepadInput, playerController)
    {
        GiveCommandToPlayer(this);
    }

    public override void Execute(InputValue value)
    {
        playerController.EndCharacterTurn();
    }
}
