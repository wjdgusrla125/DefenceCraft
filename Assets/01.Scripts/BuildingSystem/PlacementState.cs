using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlacementState : IBuildingState
{
    private int selectedObjectIndex = -1;
    private int ID;
    private Grid _grid;
    private PreviewSystem _previewSystem;
    private ObjectsDatabaseSO database;
    private GridData floorData, furnitureData;
    private ObjectPlacer _objectPlacer;

    public PlacementState(int id, Grid grid, PreviewSystem previewSystem, ObjectsDatabaseSO database, GridData floorData, GridData furnitureData, ObjectPlacer objectPlacer)
    {
        ID = id;
        _grid = grid;
        _previewSystem = previewSystem;
        this.database = database;
        this.floorData = floorData;
        this.furnitureData = furnitureData;
        _objectPlacer = objectPlacer;
        
        selectedObjectIndex = database.objectsData.FindIndex(data => data.ID == ID);
        if (selectedObjectIndex > -1)
        {
            _previewSystem.StartShowingPlacementPreview(database.objectsData[selectedObjectIndex].Prefab, database.objectsData[selectedObjectIndex].Size);
        }
        else
        {
            throw new System.Exception($"No Object with ID {id}");
        }
    }

    public void EndState()
    {
        _previewSystem.StopShowingPreview();
    }

    public void OnAction(Vector3Int gridPosition)
    {
        bool placementValidity = CheckPlacementValidity(gridPosition, selectedObjectIndex);
        if (placementValidity == false) return; //비정상효과음 추가 하는 곳
        //정상효과음 추가 하는 곳

        int index = _objectPlacer.PlaceObject(database.objectsData[selectedObjectIndex].Prefab, _grid.CellToWorld(gridPosition));
        
        GridData selectedData = database.objectsData[this.selectedObjectIndex].ID == 0 ? floorData : furnitureData;
        selectedData.AddObjectAt(gridPosition, database.objectsData[selectedObjectIndex].Size, database.objectsData[selectedObjectIndex].ID, index);
        
        _previewSystem.UpdatePosition(_grid.CellToWorld(gridPosition), false);
    }

    public void UpdateState(Vector3Int gridPosition)
    {
        bool placementValidity = CheckPlacementValidity(gridPosition, selectedObjectIndex);
        _previewSystem.UpdatePosition(_grid.CellToWorld(gridPosition), placementValidity);
    }
    
    private bool CheckPlacementValidity(Vector3Int gridPosition, int selectedObjectIndex)
    {
        GridData selectedData = database.objectsData[this.selectedObjectIndex].ID == 0 ? floorData : furnitureData;

        return selectedData.CanPlacedObjectAt(gridPosition, database.objectsData[selectedObjectIndex].Size);
    }
}
