using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class PlacementSystem : MonoBehaviour
{
    [SerializeField]
    private InputManager inputManager;
    [SerializeField]
    private Grid grid;

    [SerializeField]
    private ObjectsDatabaseSO database;

    [SerializeField]
    private GameObject gridVisualization;
    
    private GridData furnitureData;

    [SerializeField]
    private PreviewSystem preview;

    private Vector3Int lastDetectedPosition = Vector3Int.zero;

    [SerializeField]
    private ObjectPlacer objectPlacer;

    IBuildingState buildingState;
    
    private Dictionary<BuildingType, List<BuildingType>> buildingDependencies = new Dictionary<BuildingType, List<BuildingType>>
    {
        { BuildingType.Barrack, new List<BuildingType> { BuildingType.Castle } },
        { BuildingType.Wall, new List<BuildingType> { BuildingType.Barrack } },
        { BuildingType.MagicTower, new List<BuildingType> { BuildingType.Barrack } }
        
    };
    
    private void Start()
    {
        gridVisualization.SetActive(false);
        furnitureData = new();
    }

    public void StartPlacement(int ID)
    {
        StopPlacement();
        BuildingType buildingType = GetBuildingTypeFromID(ID);
        
        if (!CanPlaceBuilding(buildingType))
        {
            Debug.Log($"Cannot place {buildingType}. Required buildings are not present.");
            return;
        }
        
        gridVisualization.SetActive(true);
        buildingState = new PlacementState(ID, grid, preview, database, furnitureData, objectPlacer);
        inputManager.OnClicked += PlaceStructure;
        inputManager.OnExit += StopPlacement;
        inputManager.OnRotate += RotateStructure;

    }
    
    private bool CanPlaceBuilding(BuildingType buildingType)
    {
        if (!buildingDependencies.ContainsKey(buildingType)) return true;
        
        foreach (BuildingType requiredType in buildingDependencies[buildingType])
        {
            bool requiredBuildingExists = SelectionManager.Instance.allBuildingList
                .Any(building => building.GetComponent<Building>()?.buildingType == requiredType);

            if (!requiredBuildingExists) return false;
        }

        return true;
    }
    
    private BuildingType GetBuildingTypeFromID(int ID)
    {
        return database.objectsData.Find(data => data.ID == ID)?.Prefab.GetComponent<Building>()?.buildingType ?? BuildingType.None;
    }

    public void StartRemoving()
    {
        StopPlacement();
        gridVisualization.SetActive(true) ;
        buildingState = new RemovingState(grid, preview, furnitureData, objectPlacer);
        inputManager.OnClicked += PlaceStructure;
        inputManager.OnExit += StopPlacement;
        inputManager.OnRotate += RotateStructure;
    }

    private void PlaceStructure()
    {
        if(inputManager.IsPointerOverUI()) return;
        
        Vector3 mousePosition = inputManager.GetSelectedMapPosition();
        Vector3Int gridPosition = grid.WorldToCell(mousePosition);

        preview.StopShowingPreview();
        buildingState.OnAction(gridPosition);
    }

    private void RotateStructure()
    {
        if(inputManager.IsPointerOverUI()) return;
        
        Vector3 mousePosition = inputManager.GetSelectedMapPosition();
        Vector3Int gridPosition = grid.WorldToCell(mousePosition);

        buildingState.OnRotation(gridPosition);
    }
    
    private void StopPlacement()
    {
        if (buildingState == null) return;
        
        gridVisualization.SetActive(false);
        buildingState.EndState();
        inputManager.OnClicked -= PlaceStructure;
        inputManager.OnExit -= StopPlacement;
        inputManager.OnRotate -= RotateStructure;
        lastDetectedPosition = Vector3Int.zero;
        buildingState = null;
    }

    private void Update()
    {
        if (buildingState == null) return;
        
        Vector3 mousePosition = inputManager.GetSelectedMapPosition();
        Vector3Int gridPosition = grid.WorldToCell(mousePosition);
        
        if(lastDetectedPosition != gridPosition)
        {
            buildingState.UpdateState(gridPosition);
            lastDetectedPosition = gridPosition;
        }
    }
}