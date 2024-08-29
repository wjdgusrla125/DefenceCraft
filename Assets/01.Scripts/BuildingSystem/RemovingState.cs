/*
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RemovingState : IBuildingState
{
    private int gameObjectIndex = -1;
    private Grid _grid;
    private PreviewSystem _previewSystem;
    private GridData floorData, furnitureData;
    private ObjectPlacer _objectPlacer;

    public RemovingState(Grid grid, PreviewSystem previewSystem, GridData floorData, GridData furnitureData, ObjectPlacer objectPlacer)
    {
        _grid = grid;
        _previewSystem = previewSystem;
        this.floorData = floorData;
        this.furnitureData = furnitureData;
        _objectPlacer = objectPlacer;
        
        _previewSystem.StartShowingRemovePreview();
    }

    public void EndState()
    {
        _previewSystem.StopShowingPreview();
    }

    public void OnAction(Vector3Int gridPosition)
    {
        GridData selectedData = null;

        if (furnitureData.CanPlacedObjectAt(gridPosition,Vector2Int.one) == false)
        {
            selectedData = furnitureData;
        }
        else if(floorData.CanPlacedObjectAt(gridPosition,Vector2Int.one) == false)
        {
            selectedData = floorData;
        }

        if (selectedData == null)
        {
            Debug.Log("c");
        }
        else
        {
            gameObjectIndex = selectedData.GetRepresentationIndex(gridPosition);
            
            if(gameObjectIndex == -1) return;
            
            Debug.Log("b");

            selectedData.RemoveObjectAt(gridPosition);
            _objectPlacer.RemoveObjectAt(gameObjectIndex);
        }

        Vector3 cellPosition = _grid.CellToWorld(gridPosition);
        _previewSystem.UpdatePosition(cellPosition,CheckIfSelectionIsValid(gridPosition));
    }

    private bool CheckIfSelectionIsValid(Vector3Int gridPosition)
    {
        return !(furnitureData.CanPlacedObjectAt(gridPosition, Vector2Int.one) &&
                 floorData.CanPlacedObjectAt(gridPosition, Vector2Int.one));
    }

    public void UpdateState(Vector3Int gridPosition)
    {
        bool validity = CheckIfSelectionIsValid(gridPosition);
        _previewSystem.UpdatePosition(_grid.CellToWorld(gridPosition), validity);
    }
}
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RemovingState : IBuildingState
{
    private int gameObjectIndex = -1;
    private Grid _grid;
    private PreviewSystem _previewSystem;
    private GridData floorData, furnitureData;
    private ObjectPlacer _objectPlacer;

    public RemovingState(Grid grid, PreviewSystem previewSystem, GridData floorData, GridData furnitureData, ObjectPlacer objectPlacer)
    {
        _grid = grid;
        _previewSystem = previewSystem;
        this.floorData = floorData;
        this.furnitureData = furnitureData;
        _objectPlacer = objectPlacer;
        
        _previewSystem.StartShowingRemovePreview();
    }

    public void EndState()
    {
        _previewSystem.StopShowingPreview();
    }

    public void OnAction(Vector3Int gridPosition)
    {
        GridData selectedData = null;
        
        if (furnitureData.GetRepresentationIndex(gridPosition) != -1)
        {
            selectedData = furnitureData;
        }
        else if (floorData.GetRepresentationIndex(gridPosition) != -1)
        {
            selectedData = floorData;
        }

        if (selectedData == null)
        {
            
        }
        else
        {
            gameObjectIndex = selectedData.GetRepresentationIndex(gridPosition);
            
            if(gameObjectIndex == -1) return;
            
            selectedData.RemoveObjectAt(gridPosition);
            _objectPlacer.RemoveObjectAt(gameObjectIndex);
        }

        Vector3 cellPosition = _grid.CellToWorld(gridPosition);
        _previewSystem.UpdatePosition(cellPosition, CheckIfSelectionIsValid(gridPosition));
    }

    private bool CheckIfSelectionIsValid(Vector3Int gridPosition)
    {
        return !(furnitureData.GetRepresentationIndex(gridPosition) != -1 ||
                 floorData.GetRepresentationIndex(gridPosition) != -1);
    }

    public void UpdateState(Vector3Int gridPosition)
    {
        bool validity = CheckIfSelectionIsValid(gridPosition);
        _previewSystem.UpdatePosition(_grid.CellToWorld(gridPosition), validity);
    }
}