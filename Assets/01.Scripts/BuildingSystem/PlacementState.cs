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
    
    private Character selectedCharacter;
    private Vector3Int buildPosition;
    private GameObject buildingPrefab;
    
    private bool isBuilding = false;

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
        if (isBuilding) return;

        if (database.objectsData[selectedObjectIndex].BuildingCost > GameManager.Instance.Gold.Value)
        {
            return;
        }
        else
        {
            GameManager.Instance.Gold.ApplyChange(-1 * database.objectsData[selectedObjectIndex].BuildingCost);
        }

        bool placementValidity = CheckPlacementValidity(gridPosition, selectedObjectIndex);
        
        if (placementValidity == false) return;

        buildPosition = gridPosition;
        buildingPrefab = database.objectsData[selectedObjectIndex].Prefab;
        
        foreach (GameObject unit in SelectionManager.Instance.unitSelected)
        {
            Character character = unit.GetComponent<Character>();
            if (character != null)
            {
                selectedCharacter = character;
                break;
            }
        }

        if (selectedCharacter != null)
        {
            isBuilding = true;
            selectedCharacter.SetDestination(grid.CellToWorld(gridPosition));
            selectedCharacter.Fsm.ChangeState(FSM_CharacterState.FSM_CharacterState_Move);
            
            selectedCharacter.StartCoroutine(WaitForCharacterArrival());
        }
    }
    
    private IEnumerator WaitForCharacterArrival()
    {
        while (Vector3.Distance(selectedCharacter.transform.position, grid.CellToWorld(buildPosition)) > 0.1f)
        {
            yield return null;
        }
        
        selectedCharacter.Fsm.ChangeState(FSM_CharacterState.FSM_CharacterState_Build);
        selectedCharacter.StartBuilding();
        
        GameObject buildingInstance = Object.Instantiate(buildingPrefab, grid.CellToWorld(buildPosition), Quaternion.identity);
        Building buildingComponent = buildingInstance.GetComponent<Building>();
        float constructionTime = buildingComponent.GetConstructionTime();
        
        Object.Destroy(buildingInstance);
        
        float elapsedTime = 0f;
        while (elapsedTime < constructionTime)
        {
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        
        int index = objectPlacer.PlaceObject(buildingPrefab, grid.CellToWorld(buildPosition), currentRotation);
        
        GridData selectedData = furnitureData;
        selectedData.AddObjectAt(buildPosition, database.objectsData[selectedObjectIndex].Size, database.objectsData[selectedObjectIndex].ID, index, currentRotation);
        
        previewSystem.UpdatePosition(grid.CellToWorld(buildPosition), false, currentRotation);
        
        selectedCharacter.StopBuilding();
        selectedCharacter.Fsm.ChangeState(FSM_CharacterState.FSM_CharacterState_Idle);
        
        isBuilding = false;
        selectedCharacter = null;
        buildPosition = Vector3Int.zero;
        buildingPrefab = null;
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
}