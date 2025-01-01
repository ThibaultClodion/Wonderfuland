using IA;
using System.Collections.Generic;
using UnityEngine;
using Controller;

class MovementManager
{
    public static void MoveCharacter(Vector2Int coordinate, PlayerController playerController)
    {
        //If the Player is choosing a SpawnPosition and it's valid OR is not choosing a spawn position
        if (GameState.actualState != GameState.State.ChoosingSpawnPosition || (!playerController.hasSpawnSet && FightManager.spawnManagement.IsAValidSpawnPosition(coordinate)))
        {
            MoveCharacterTo(playerController.GetCurrentCharacter(), coordinate, playerController.GetCurrentCharacterGO().transform);
        }
    }

    public static void MoveCharacter(Vector2Int coordinate, IAController IAController, int index)
    {
        MoveCharacterTo(IAController.characters[index], coordinate, IAController.charactersGO[index].transform);
    }

    public static Vector2Int CorrectInputAccordingToCameras(Vector2Int inputVector)
    {
        Transform cameraTransform = MapManager.Instance.currentMap.GetActiveCamera().transform;

        if (45 <= cameraTransform.rotation.eulerAngles.y % 360 && cameraTransform.rotation.eulerAngles.y % 360 <= 130)
        {
            return new Vector2Int(inputVector.y, -inputVector.x);
        }
        else if (130 <= cameraTransform.rotation.eulerAngles.y % 360 && cameraTransform.rotation.eulerAngles.y % 360 <= 225)
        {
            return -inputVector;
        }
        else if (225 <= cameraTransform.rotation.eulerAngles.y % 360 && cameraTransform.rotation.eulerAngles.y % 360 <= 315)
        {
            return new Vector2Int(-inputVector.y, inputVector.x);
        }
        else
        {
            return inputVector;
        }
    }

    private static void MoveCharacterTo(Character character, Vector2Int desiredPosition, Transform transform)
    {
        if (GameState.actualState == GameState.State.ChoosingSpawnPosition)
        {
            InstantMovement(character, desiredPosition, transform);
        }
        else if (IsAValidPosition(desiredPosition) && GameState.actualState != GameState.State.ChoosingSpawnPosition)
        {
            Cell[][] map = MapManager.Instance.GetGrid();
            List<Vector2Int> path = AStar.execute(map, character.position, desiredPosition);

            //Decompose the mouvement step by step in case it need to break because the character walk on a invisible effect
            foreach (Vector2Int movement in path)
            {
                OneMovement(character, movement, transform);
            }
        }
    }

    public static void MoveVisualisationEffect(GameObject visualisationGO, Vector2Int desiredPosition)
    {
        if(MapManager.Instance.IsNotEmpty(desiredPosition))
        {
            visualisationGO.transform.localPosition = new Vector3(desiredPosition.x, visualisationGO.transform.localPosition.y, desiredPosition.y);
        }
    }

    private static bool IsAValidPosition(Vector2Int position)
    {
        if (MapManager.Instance.GetCell(position).GetCellType() == Cell.CellType.Ground && HasNoCharacterOnPosition(position))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private static bool HasNoCharacterOnPosition(Vector2Int position)
    {
        foreach(Character character in CharacterManager.GetCharacters()) 
        {
            if(character.position == position) 
            {
                return false;
            }
        }
        return true;
    }

    private static void InstantMovement(Character character, Vector2Int newPosition, Transform transform)
    {
        //Update the position of the character
        character.position = newPosition;

        //Make the character move
        transform.localPosition = new Vector3(newPosition.x, transform.localPosition.y, newPosition.y);
    }

    private static void OneMovement(Character character, Vector2Int newPosition, Transform transform)
    {
        //Update the position of the character
        character.position = newPosition;

        //Make the character move and lose his pm. (if it's with pm)
        transform.localPosition = new Vector3(newPosition.x, transform.localPosition.y, newPosition.y);

        //Apply the effect of the new Cell
        MapManager.Instance.ApplyWalkOnCellEffect(character);

    }

    private static List<Vector2Int> DumbPath(Vector2Int startPosition, Vector2Int endPosition)
    {
        List<Vector2Int> path = new List<Vector2Int>();

        //X-axis movement
        if (startPosition.x < endPosition.x)
        {
            for (int x = startPosition.x; x < endPosition.x; x++)
            {
                path.Add(new Vector2Int(x, startPosition.y));
            }
        }
        else if (startPosition.x > endPosition.x)
        {
            for (int x = startPosition.x; x > endPosition.x; x--)
            {
                path.Add(new Vector2Int(x, startPosition.y));
            }
        }

        //Y-axis Movement
        if (startPosition.y < endPosition.y)
        {
            for (int y = startPosition.y; y < endPosition.y; y++)
            {
                path.Add(new Vector2Int(endPosition.x, y));
            }
        }
        else if (startPosition.y > endPosition.y)
        {
            for (int y = startPosition.y; y > endPosition.y; y--)
            {
                path.Add(new Vector2Int(endPosition.x, y));
            }
        }

        //Add the last cell
        path.Add(new Vector2Int(endPosition.x, endPosition.y));
        path.RemoveAt(0);

        return path;
    }
}
