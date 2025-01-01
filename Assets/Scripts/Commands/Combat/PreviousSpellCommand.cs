using UnityEngine.InputSystem;
using Controller;

class PreviousSpellCommand : BaseCommand
{
    public PreviousSpellCommand(PlayerInputs keyboardInput, PlayerInputs gamepadInput, PlayerController playerController) :
        base(keyboardInput, gamepadInput, playerController)
    {
        GiveCommandToPlayer(this);
    }

    public override void Execute(InputValue value)
    {
        playerController.PreviousSpell();
    }
}
