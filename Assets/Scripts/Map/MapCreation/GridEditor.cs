using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GridEditor : MonoBehaviour
{
    public Tilemap tilemap;
    private GridLayout gridLayout;
    public GameObject ground;

    private void Start()
    {
        gridLayout = GetComponent<Grid>();
        Debug.Log(tilemap.GetInstantiatedObject(new Vector3Int(0, 0,0)));
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out RaycastHit hitInfo, 100f))
            {
                //Instantiate the new GameObject
                Vector3 gridCell = gridLayout.WorldToCell(hitInfo.transform.position);
                Debug.Log(gridCell);
                Vector3Int gridCellInt = Vector3Int.FloorToInt(gridCell);
                GameObject newGameObject = Instantiate(ground, tilemap.transform);
                newGameObject.transform.position = gridLayout.CellToWorld(gridCellInt);
                Debug.Log(gridLayout.CellToWorld(gridCellInt));

                // Destroy GameObject hit
                Destroy(hitInfo.collider.gameObject);

            }
        }
    }
}
