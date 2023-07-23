using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlacementSystem : MonoBehaviour
{
    [SerializeField] private GameObject mouseIndicator;
    [SerializeField] private InputManager inputManager;
    [SerializeField] private GameObject cellIndicator;
    [SerializeField] private Grid grid;

    [SerializeField] private ObjectsDatabaseSO database;
    [SerializeField] private int selectedObjectIndex = -1;
    [SerializeField] private GameObject gridVisualization;
    private void Start()
    {
        StopPlacement();
    }
    public void StartPlacement(int ID)
    {
        StopPlacement();
        selectedObjectIndex = database.objectsData.FindIndex(data => data.ID == ID);
        if(selectedObjectIndex < 0)
        {
            Debug.LogError($"No ID found {ID}");
            return;
        }
        gridVisualization.SetActive(true);
        cellIndicator.SetActive(true);
        inputManager.OnClicked += PlaceStructure;
        inputManager.OnExit += StopPlacement;
    }
    public void StopPlacement()
    {
        selectedObjectIndex = -1;
        gridVisualization.SetActive(false);
        cellIndicator.SetActive(false);
        inputManager.OnClicked -= PlaceStructure;
        inputManager.OnExit -= StopPlacement;
    }
    public void PlaceStructure()
    {
        if(inputManager.IsPointerOverUI())
        {
            return;
        }
        Vector3 mousePosition = inputManager.GetSelectedMousePosition();
        Vector3Int gridPosition = grid.WorldToCell(mousePosition);
        GameObject gameObject = Instantiate(database.objectsData[selectedObjectIndex].Prefab);
        gameObject.transform.position = grid.CellToWorld(gridPosition);
    }


    private void Update()
    {
        if (selectedObjectIndex < 0) { return; }
        Vector3 mousePosition = inputManager.GetSelectedMousePosition();
        Vector3Int gridPosition= grid.WorldToCell(mousePosition);
        mouseIndicator.transform.position = mousePosition;
        cellIndicator.transform.position = grid.CellToWorld(gridPosition);

    }

}
