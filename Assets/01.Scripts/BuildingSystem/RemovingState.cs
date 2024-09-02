using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RemovingState : IBuildingState
{
    private int gameObjectIndex = -1;
    Grid grid;
    PreviewSystem previewSystem;
    GridData furnitureData;
    ObjectPlacer objectPlacer;

    public RemovingState(Grid grid, PreviewSystem previewSystem, GridData furnitureData, ObjectPlacer objectPlacer)
    {
        this.grid = grid;
        this.previewSystem = previewSystem;
        this.furnitureData = furnitureData;
        this.objectPlacer = objectPlacer;
        previewSystem.StartShowingRemovePreview();
    }

    public void EndState()
    {
        previewSystem.StopShowingPreview();
    }

    public void OnAction(Vector3Int gridPosition)
    {
        GridData selectedData = null;
        
        if(furnitureData.CanPlaceObejctAt(gridPosition,Vector2Int.one, 0) == false)
        {
            selectedData = furnitureData;
        }

        if(selectedData != null)
        {
            gameObjectIndex = selectedData.GetRepresentationIndex(gridPosition);
            
            if (gameObjectIndex == -1) return;
            
            selectedData.RemoveObjectAt(gridPosition);
            objectPlacer.RemoveObjectAt(gameObjectIndex);
        }
        
        Vector3 cellPosition = grid.CellToWorld(gridPosition);
        previewSystem.UpdatePosition(cellPosition, CheckIfSelectionIsValid(gridPosition), 0);
    }

    private bool CheckIfSelectionIsValid(Vector3Int gridPosition)
    {
        return !(furnitureData.CanPlaceObejctAt(gridPosition, Vector2Int.one, 0));
    }

    public void UpdateState(Vector3Int gridPosition)
    {
        bool validity = CheckIfSelectionIsValid(gridPosition);
        previewSystem.UpdatePosition(grid.CellToWorld(gridPosition), validity, 0);
    }
    
    public void OnRotation(Vector3Int gridPosition)
    {
        
    }
}