using System.Collections.Generic;
using UnityEngine;
using SpellCreator;
using UnityEngine.Tilemaps;

class Map : MonoBehaviour
{
    public Tilemap tilemap;
    private Cell[][] grid;

    //Events of the map which are just spell casted
    public List<Spell> spells; // Comment on peut récupérer les scripts en scriptable ? Il faut que cela soit des scriptableObject je crois.
                               // + Il faudrait gérer les tours de lancement.

    //Spawn positions
    public Vector2Int[] spawnPositions;

    [Header("Ennemies")]
    //We could group these two datas on a class Ennemies to be "clearer" on which character is at which position
    public CharacterData[] ennemies;
    public Vector2Int[] ennemiesPosition;

    [System.Serializable]
    public class HiddenGameObjectCamera
    {
        public Camera camera;
        public GameObject[] gameObjectsToHide;
    }

    [Header("Cameras")]
    [SerializeField] private HiddenGameObjectCamera[] cameras;
    private int indexActualCamera = 0;

    [Header("Game Objects")]
    [SerializeField] private GameObject[] goToEnableInCombat;
    [SerializeField] private GameObject[] goToDisableInCombat;

    [Header("End of Combat")]
    [SerializeField] private Transform wagonTransform;
    [SerializeField] private Transform endOfCombatWagonPosition;
    public Vector2Int[] endCombatPositions;

    public Cell[][] GetGrid()
    {
        InitGrid();

        Transform[] cells = tilemap.gameObject.GetComponentsInChildren<Transform>();

        //Get the type of cells
        foreach (Transform cell in cells)
        {
            if(IsABasicBlock(cell))
            {
                SetCellType(cell);
            }
        }
        return grid;
    }

    private int FindLength()
    {
        int length = 0;

        Transform[] tiles = tilemap.gameObject.GetComponentsInChildren<Transform>();

        foreach (Transform t in tiles)
        {
            if (t.localPosition.z >= length && IsABasicBlock(t))
            {
                length = Mathf.CeilToInt(t.localPosition.z);
            }
        }

        return length + 1;
    }

    private int FindWidth()
    {
        int width = 0;

        Transform[] tiles = tilemap.gameObject.GetComponentsInChildren<Transform>();

        foreach (Transform t in tiles)
        {
            if (t.localPosition.x >= width && IsABasicBlock(t))
            {
                width = Mathf.CeilToInt(t.localPosition.x);
            }
        }

        return width + 1;
    }

    private bool IsABasicBlock(Transform cell)
    {
        return cell.tag == TagManager.GROUND_TAG
            || cell.tag == TagManager.HOLE_TAG
            || cell.tag == TagManager.WALL_TAG;
    }

    private void SetCellType(Transform cell)
    {
        if (cell.tag == TagManager.GROUND_TAG)
        {
            SetType(cell.localPosition, Cell.CellType.Ground);
        }
        else if (cell.tag == TagManager.HOLE_TAG)
        {
            SetType(cell.localPosition, Cell.CellType.Hole);
        }
        else if (cell.tag == TagManager.WALL_TAG)
        {
            SetType(cell.localPosition, Cell.CellType.Wall);
        }
    }

    private void SetType(Vector3 pos, Cell.CellType cellType)
    {
        if(0 <= pos.x && pos.x < grid.Length && 0 <= pos.z && pos.z < grid[0].Length)
        {
            grid[Mathf.FloorToInt(pos.x)][Mathf.FloorToInt(pos.z)].SetType(cellType);
        }
    }

    private void InitGrid()
    {
        int length = FindLength();
        int width = FindWidth();

        grid = new Cell[width][];

        for (int i = 0; i < width; i++)
        {
            grid[i] = new Cell[length];

            for (int j = 0; j < length; j++)
            {
                grid[i][j] = new Cell();
            }
        }
    }

    public void StartCombat()
    {
        ChangeGOActive(goToEnableInCombat, true);
        ChangeGOActive(goToDisableInCombat, false);
        ResetCameras();
    }

    public void EndCombat()
    {
        ChangeGOActive(goToEnableInCombat, false);
        ChangeGOActive(goToDisableInCombat, true);

        wagonTransform.position = endOfCombatWagonPosition.position;
    }

    private void ChangeGOActive(GameObject[] gameObjects, bool isActive)
    {
        foreach (GameObject go in gameObjects)
        {
            go.SetActive(isActive);
        }
    }

    private void ResetCameras()
    {
        for(int i = 1; i < cameras.Length;i++)
        {
            foreach (GameObject go in cameras[i].gameObjectsToHide)
            {
                go.SetActive(true);
            }
            ChangeCameraActivity(cameras[i].camera, false);
        }

        foreach (GameObject go in cameras[0].gameObjectsToHide)
        {
            go.SetActive(false);
        }
        ChangeCameraActivity(cameras[0].camera, true);

        indexActualCamera = 0;
    }

    public void SwitchActiveCamera()
    {
        //Update hidden GameObjects to hide
        foreach (GameObject go in cameras[(indexActualCamera + 1) % cameras.Length].gameObjectsToHide)
        {
            go.SetActive(false);
        }
        foreach (GameObject go in cameras[indexActualCamera].gameObjectsToHide)
        {
            go.SetActive(true);
        }

        //Update visible camera
        ChangeCameraActivity(cameras[(indexActualCamera + 1) % cameras.Length].camera, true);
        ChangeCameraActivity(cameras[indexActualCamera].camera, false);

        //Change Index
        indexActualCamera = (indexActualCamera + 1) % cameras.Length;
    }

    private void ChangeCameraActivity(Camera camera, bool isActive)
    {
        camera.gameObject.SetActive(isActive);
        camera.enabled = isActive;
    }

    public Camera GetActiveCamera()
    {
        return cameras[indexActualCamera].camera;
    }
}
