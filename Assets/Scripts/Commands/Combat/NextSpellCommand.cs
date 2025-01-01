using UnityEngine.InputSystem;
using Controller;

class NextSpellCommand : BaseCommand
{

    public NextSpellCommand(PlayerInputs keyboardInput, PlayerInputs gamepadInput, PlayerController playerController) :
        base(keyboardInput, gamepadInput, playerController)
    {
        GiveCommandToPlayer(this);
    }

    public override void Execute(InputValue value)
    {
        playerController.NextSpell();
    }
}
