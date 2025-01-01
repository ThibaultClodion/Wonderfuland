using UnityEngine;
using UnityEngine.InputSystem;
using Controller;

class KeyboardChoosingPositionMoveCommand : BaseCommand
{
    public KeyboardChoosingPositionMoveCommand(PlayerInputs keyboardInput, PlayerInputs gamepadInput, PlayerController playerController) :
        base(keyboardInput, gamepadInput, playerController)
    { GiveCommandToPlayer(this); }

    public override void Execute(InputValue value)
    {
        if(value.Get<Vector2>() != Vector2.zero)
        {
            Ray ray = Camera.main.ScreenPointToRay(value.Get<Vector2>());

            if (Physics.Raycast(ray, out RaycastHit hitInfo, 100f, ~(1 << 0)))
            {
                Vector2Int coordinate = MapManager.Instance.WorldToCoordinate(hitInfo.transform.localPosition);

                MovementManager.MoveCharacter(coordinate, playerController);
            }
        }
    }
}
