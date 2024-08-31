/*using UnityEngine;

public class PlacementState : IBuildingState
{
    private int selectedObjectIndex = -1;
    private int ID;
    private Grid _grid;
    private PreviewSystem _previewSystem;
    private ObjectsDatabaseSO database;
    private GridData floorData, furnitureData;
    private ObjectPlacer _objectPlacer;
    private Vector2Int selectedObjectSize;
    private Vector2Int rotatedSize;

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
            selectedObjectSize = database.objectsData[selectedObjectIndex].Size;
            rotatedSize = selectedObjectSize;  // 초기에는 회전되지 않은 상태
            _previewSystem.StartShowingPlacementPreview(database.objectsData[selectedObjectIndex].Prefab, selectedObjectSize);
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
        if (placementValidity == false) return;

        if (GameManager.Instance.Gold.Value < database.objectsData[selectedObjectIndex].Cost)
        {
            return;
        }
        else
        {
            GameManager.Instance.Gold.ApplyChange(-1 * database.objectsData[selectedObjectIndex].Cost);
        }
        
        int index = _objectPlacer.PlaceObject(database.objectsData[selectedObjectIndex].Prefab, _grid.CellToWorld(gridPosition));
        
        GridData selectedData = database.objectsData[selectedObjectIndex].ID == 0 ? floorData : furnitureData;
        selectedData.AddObjectAt(gridPosition, rotatedSize, database.objectsData[selectedObjectIndex].ID, index);
        
        _previewSystem.UpdatePosition(_grid.CellToWorld(gridPosition), false);
    }

    public void UpdateState(Vector3Int gridPosition)
    {
        bool placementValidity = CheckPlacementValidity(gridPosition, selectedObjectIndex);
        _previewSystem.UpdatePosition(_grid.CellToWorld(gridPosition), placementValidity);
    }
    
    private bool CheckPlacementValidity(Vector3Int gridPosition, int selectedObjectIndex)
    {
        GridData selectedData = database.objectsData[selectedObjectIndex].ID == 0 ? floorData : furnitureData;
        return selectedData.CanPlacedObjectAt(gridPosition, rotatedSize);
    }

    public Vector2Int GetSelectedObjectSize()
    {
        return selectedObjectSize;
    }

    public void SetRotatedSize(Vector2Int newSize)
    {
        rotatedSize = newSize;
    }
}*/

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
    private Vector2Int selectedObjectSize;
    private Vector2Int rotatedSize;
    //private SelectionManager _selectionManager;

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
            selectedObjectSize = database.objectsData[selectedObjectIndex].Size;
            rotatedSize = selectedObjectSize;  // 초기에는 회전되지 않은 상태
            _previewSystem.StartShowingPlacementPreview(database.objectsData[selectedObjectIndex].Prefab, selectedObjectSize);
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
        if (placementValidity == false) return;

        if (GameManager.Instance.Gold.Value < database.objectsData[selectedObjectIndex].Cost)
        {
            return;
        }
        else
        {
            GameManager.Instance.Gold.ApplyChange(-1 * database.objectsData[selectedObjectIndex].Cost);
        }
        
        if (SelectionManager.Instance.unitSelected.Count > 0)
        {
            Character builder = SelectionManager.Instance.unitSelected[0].GetComponent<Character>();
            
            if (builder != null)
            {
                // Start the building process
                builder.BuildStructure(database.objectsData[selectedObjectIndex].Prefab, _grid.CellToWorld(gridPosition));
                
                GridData selectedData = database.objectsData[selectedObjectIndex].ID == 0 ? floorData : furnitureData;
                selectedData.AddObjectAt(gridPosition, rotatedSize, database.objectsData[selectedObjectIndex].ID, -1); // Use -1 as a temporary index
                
                _previewSystem.UpdatePosition(_grid.CellToWorld(gridPosition), false);
            }
        }
    }

    public void UpdateState(Vector3Int gridPosition)
    {
        bool placementValidity = CheckPlacementValidity(gridPosition, selectedObjectIndex);
        _previewSystem.UpdatePosition(_grid.CellToWorld(gridPosition), placementValidity);
    }
    
    private bool CheckPlacementValidity(Vector3Int gridPosition, int selectedObjectIndex)
    {
        GridData selectedData = database.objectsData[selectedObjectIndex].ID == 0 ? floorData : furnitureData;
        return selectedData.CanPlacedObjectAt(gridPosition, rotatedSize);
    }

    public Vector2Int GetSelectedObjectSize()
    {
        return selectedObjectSize;
    }

    public void SetRotatedSize(Vector2Int newSize)
    {
        rotatedSize = newSize;
    }
}