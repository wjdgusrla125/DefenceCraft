using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlacementState : IBuildingState
{
    private int selectedObjectIndex = -1;
    int ID;
    Grid grid;
    PreviewSystem previewSystem;
    ObjectsDatabaseSO database;
    GridData furnitureData;
    ObjectPlacer objectPlacer;
    
    private int currentRotation = 0;

    public PlacementState(int iD, Grid grid, PreviewSystem previewSystem, ObjectsDatabaseSO database, GridData furnitureData, ObjectPlacer objectPlacer)
    {
        ID = iD;
        this.grid = grid;
        this.previewSystem = previewSystem;
        this.database = database;
        this.furnitureData = furnitureData;
        this.objectPlacer = objectPlacer;

        selectedObjectIndex = database.objectsData.FindIndex(data => data.ID == ID);
        
        if (selectedObjectIndex > -1)
        {
            previewSystem.StartShowingPlacementPreview(database.objectsData[selectedObjectIndex].Prefab, database.objectsData[selectedObjectIndex].Size , currentRotation);
        }
    }

    public void EndState()
    {
        previewSystem.StopShowingPreview();
    }

    public void OnAction(Vector3Int gridPosition)
    {
        bool placementValidity = CheckPlacementValidity(gridPosition, selectedObjectIndex);
        
        if (placementValidity == false) return;
        
        int index = objectPlacer.PlaceObject(database.objectsData[selectedObjectIndex].Prefab, grid.CellToWorld(gridPosition), currentRotation);
        
        GridData selectedData = furnitureData;
        
        selectedData.AddObjectAt(gridPosition, database.objectsData[selectedObjectIndex].Size, database.objectsData[selectedObjectIndex].ID, index, currentRotation);
        
        previewSystem.UpdatePosition(grid.CellToWorld(gridPosition), false, currentRotation);
    }

    public void OnRotation(Vector3Int gridPosition)
    {
        currentRotation = (currentRotation + 1) % 4;
        previewSystem.UpdatePosition(grid.CellToWorld(gridPosition), CheckPlacementValidity(gridPosition, selectedObjectIndex), currentRotation);
    }

    private bool CheckPlacementValidity(Vector3Int gridPosition, int selectedObjectIndex)
    {
        GridData selectedData = furnitureData;

        return furnitureData.CanPlaceObejctAt(gridPosition, database.objectsData[selectedObjectIndex].Size, currentRotation);
    }

    public void UpdateState(Vector3Int gridPosition)
    {
        bool placementValidity = CheckPlacementValidity(gridPosition, selectedObjectIndex);

        previewSystem.UpdatePosition(grid.CellToWorld(gridPosition), placementValidity, currentRotation);
    }
    
    // private Vector2Int GetRotatedSize(Vector2Int originalSize)
    // {
    //     if (currentRotation % 2 == 0)
    //     {
    //         return originalSize;
    //     }
    //     
    //     return new Vector2Int(originalSize.y, originalSize.x);
    // }
}