using UnityEngine.InputSystem;
using Controller;

class KeyboardLaunchSpell : BaseCommand
{
    public KeyboardLaunchSpell(PlayerInputs keyboardInput, PlayerInputs gamepadInput, PlayerController playerController) :
        base(keyboardInput, gamepadInput, playerController)
    { GiveCommandToPlayer(this); }

    public override void Execute(InputValue value)
    {
        playerController.LaunchSpell();
    }
}
