using SpellCreator;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Assertions;

class MapManager : Singleton<MapManager>
{
    private Cell[][] grid;
    [SerializeField] public GridLayout gridLayout;
    [NonSerialized] public Map currentMap;

    protected override void Awake()
    {
        base.Awake();   //Call the Awake Singleton Method
    }

    public void ChangeMap(Map map)
    {
        currentMap = map;
        map.StartCombat();
        InitGrid();
    }

    public void ChangeMapFromPrefab(Map map)
    {
        currentMap = Instantiate(map);
        map.StartCombat();
        InitGrid();
    }

    public void EndCombat()
    {
        ClearEffects();
        currentMap.EndCombat();
    }

    public void ClearEffects()
    {
        for (int x = 0; x < GetWidth(); x++)
        {
            for (int y = 0; y < GetLength(); y++)
            {
                grid[x][y].ResetEffects();
            }
        }
    }

    public void PutGameObjectOnCurrentMap(GameObject gameObject, Vector2Int positionOnMap, float yPosition = 0.01f)
    {
        /* Old Code that don't work sometimes on character that stay to position and not localPosition
        gameObject.transform.SetParent(currentMap.transform);
        gameObject.transform.localPosition = new Vector3(positionOnMap.x, yPosition, positionOnMap.y);*/

        StartCoroutine(TestMap(gameObject, positionOnMap, yPosition));
    }

    IEnumerator TestMap(GameObject gameObject, Vector2Int positionOnMap, float yPosition = 0.01f)
    {
        yield return new WaitForFixedUpdate();  //Mandatory to avoid bugs

        gameObject.transform.SetParent(currentMap.transform);

        /* In case we face velocity probel when Instantiate on a map
        if (gameObject.GetComponent<Rigidbody>() != null)
        {
            gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
        }*/
        gameObject.transform.localPosition = new Vector3(positionOnMap.x, yPosition, positionOnMap.y);
    }

    #region GridManagement
    public Cell[][] GetGrid()
    {
        return grid;
    }

    public bool IsOnGridPosition(Vector2Int pos)
    {
        return pos.x < GetWidth() && pos.x >= 0 && pos.y < GetLength() && pos.y >= 0;
    }

    public bool IsNotEmpty(Vector2Int pos)
    {
        return IsOnGridPosition(pos) && grid[pos.x][pos.y].GetCellType() != Cell.CellType.Empty;
    }

    public Cell GetCell(Vector2Int pos)
    {
        if (IsOnGridPosition(pos))
        {
            return grid[pos.x][pos.y];
        }
        else
        {
            return null;
        }
    }
    public int GetWidth()
    {
        Assert.IsNotNull(grid);
        return grid.Length;
    }

    public int GetLength()
    {
        Assert.IsNotNull(grid[0]);
        return grid[0].Length;
    }

    private void InitGrid()
    {
        grid = currentMap.GetGrid();
    }

    public Vector2Int WorldToCoordinate(Vector3 worldPosition)
    {
        Vector3 coordinate = gridLayout.WorldToCell(worldPosition);

        return new Vector2Int((int)coordinate.x, (int)coordinate.y);
    }

    #endregion

    #region EffectProc

    public void ApplyStartTurnEffects(Character character)
    {
        grid[character.position[0]][character.position[1]].ApplyStartTurnEffects(character);
    }

    public void ApplyWalkOnCellEffect(Character character)
    {
        grid[character.position.x][character.position.y].ApplyWalkOnEffects(character);
    }

    public void ApplyEndTurnEffects(Character character)
    {
        grid[character.position.x][character.position.y].ApplyEndTurnEffects(character);
    }

    #endregion

    #region EndTurn

    public void EndTurn()
    {
        EndCellsTurn();
        EndMapSpellTurn();
    }

    private void EndCellsTurn()
    {
        foreach (Cell[] row in grid)
        {
            foreach (Cell cell in row)
            {
                cell.EndTurn();
            }
        }
    }

    private void EndMapSpellTurn()
    {
        foreach (Spell spell in currentMap.spells)
        {
            //spell.Launch();
        }
    }
    #endregion
}
