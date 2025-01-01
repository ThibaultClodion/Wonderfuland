using UnityEngine;
using UnityEngine.InputSystem;
using Controller;

class StickChoosingSpawnMoveCommand : BaseCommand
{
    public StickChoosingSpawnMoveCommand(PlayerInputs keyboardInput, PlayerInputs gamepadInput, PlayerController playerController) :
        base(keyboardInput, gamepadInput, playerController)
    { GiveCommandToPlayer(this); }

    public override void Execute(InputValue value)
    {
        //Find nearest spawning position
        Vector2Int coordinate = FightManager.spawnManagement.GetGamepadSpawnPosition(Vector2Int.RoundToInt(value.Get<Vector2>()), playerController.GetCurrentCharacter().position);
        MovementManager.MoveCharacter(coordinate, playerController);
    }
}
