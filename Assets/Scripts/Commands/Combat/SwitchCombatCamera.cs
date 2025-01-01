using Controller;
using UnityEngine.InputSystem;

class SwitchCombatCamera : BaseCommand
{
    public SwitchCombatCamera(PlayerInputs keyboardInput, PlayerInputs gamepadInput, PlayerController playerController) : 
        base(keyboardInput, gamepadInput, playerController){GiveCommandToPlayer(this);}

    public override void Execute(InputValue value)
    {
        MapManager.Instance.currentMap.SwitchActiveCamera();
    }
}
