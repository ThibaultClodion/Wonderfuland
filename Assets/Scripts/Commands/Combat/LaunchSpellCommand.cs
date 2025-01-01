using UnityEngine.InputSystem;
using Controller;

class LaunchSpellCommand : BaseCommand
{
    
    public LaunchSpellCommand(PlayerInputs keyboardInput, PlayerInputs gamepadInput, PlayerController playerController) :
        base(keyboardInput, gamepadInput, playerController)
    {
        GiveCommandToPlayer(this);
    }

    public override void Execute(InputValue value)
    {
        playerController.LaunchSpell();
    }
}
