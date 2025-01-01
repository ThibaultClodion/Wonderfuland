using Controller;
using UnityEngine.InputSystem;

class ReadyCommand : BaseCommand
{
    public ReadyCommand(PlayerInputs keyboardInput, PlayerInputs gamepadInput, PlayerController playerController) :
        base(keyboardInput, gamepadInput, playerController)
    { GiveCommandToPlayer(this); }

    public override void Execute(InputValue value)
    {
        if (GameState.actualState == GameState.State.ChoosingSpawnPosition && playerController.hasSpawnSet == false)
        {
            SpawnManagement.ConfirmSpawnPosition();
            playerController.hasSpawnSet = true;
        }
    }
}
