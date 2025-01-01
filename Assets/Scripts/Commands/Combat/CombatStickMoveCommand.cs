using UnityEngine;
using UnityEngine.InputSystem;
using Controller;

class CombatStickMoveCommand : BaseCommand
{
    public CombatStickMoveCommand(PlayerInputs keyboardInput, PlayerInputs gamepadInput, PlayerController playerController) :
        base(keyboardInput, gamepadInput, playerController)
    { GiveCommandToPlayer(this); }

    public override void Execute(InputValue value)
    {
        Debug.Log(value.Get<Vector2>() + "to" + Vector2Int.RoundToInt(value.Get<Vector2>() * 1.49f));
        Vector2Int valueVector = Vector2Int.RoundToInt(value.Get<Vector2>() * 1.49f);
        valueVector = MovementManager.CorrectInputAccordingToCameras(valueVector);

        playerController.spellVisualisationMovement = valueVector;
    }
}
