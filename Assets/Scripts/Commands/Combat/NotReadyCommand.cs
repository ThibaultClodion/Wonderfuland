using UnityEngine.InputSystem;
using Controller;

class NotReadyCommand : BaseCommand
{
    public NotReadyCommand(PlayerInputs keyboardInput, PlayerInputs gamepadInput, PlayerController playerController) :
        base(keyboardInput, gamepadInput, playerController)
    { GiveCommandToPlayer(this); }

    public override void Execute(InputValue value)
    {
        if (GameState.actualState == GameState.State.ChoosingSpawnPosition && playerController.hasSpawnSet == true)
        {
            SpawnManagement.UnconfirmSpawnPosition();
            playerController.hasSpawnSet = false;
        }
    }
}
