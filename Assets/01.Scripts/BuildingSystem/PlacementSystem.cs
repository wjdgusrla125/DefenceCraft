using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlacementSystem : MonoBehaviour
{
    [SerializeField] private GameObject gridVisualization;
    [SerializeField] private InputManager _inputManager;
    [SerializeField] private PreviewSystem _previewSystem;
    [SerializeField] private Grid _grid;
    [SerializeField] private ObjectPlacer _objectPlacer;
    [SerializeField] private ObjectsDatabaseSO database;

    private GridData floorData, furnitureData;
    private Renderer previewRenderer;

    private Vector3Int lastDetectedPosition = Vector3Int.zero;

    private IBuildingState buildingState;

    private void Start()
    {
        StopPlacement();
        floorData = new GridData();
        furnitureData = new GridData();
    }

    private void Update()
    {
        if (buildingState == null) return;

        // 마우스 오른쪽 클릭 감지 및 배치 취소 처리
        if (Input.GetMouseButtonDown(1)) // 1은 마우스 오른쪽 클릭
        {
            StopPlacement();
            return;
        }

        Vector3 mousePosition = _inputManager.GetSelectedMapPosition();
        Vector3Int gridPosition = _grid.WorldToCell(mousePosition);

        if (lastDetectedPosition != gridPosition)
        {
            buildingState.UpdateState(gridPosition);
            lastDetectedPosition = gridPosition;
        }
    }

    public void StartPlacement(int ID)
    {
        StopPlacement();
        gridVisualization.SetActive(true);

        buildingState = new PlacementState(ID, _grid, _previewSystem, database, floorData, furnitureData, _objectPlacer);

        _inputManager.OnClicked += PlaceStructure;
        _inputManager.OnExit += StopPlacement;
    }

    public void StartRemoving()
    {
        StopPlacement();
        gridVisualization.SetActive(true);
        buildingState = new RemovingState(_grid, _previewSystem, floorData, furnitureData, _objectPlacer);

        _inputManager.OnClicked += PlaceStructure;
        _inputManager.OnExit += StopPlacement;
    }

    private void StopPlacement()
    {
        if (buildingState == null) return;

        gridVisualization.SetActive(false);

        buildingState.EndState();

        _inputManager.OnClicked -= PlaceStructure;
        _inputManager.OnExit -= StopPlacement;

        lastDetectedPosition = Vector3Int.zero;

        buildingState = null;
    }

    private void PlaceStructure()
    {
        if (_inputManager.IsPointerOverUI()) return;

        Vector3 mousePosition = _inputManager.GetSelectedMapPosition();
        Vector3Int gridPosition = _grid.WorldToCell(mousePosition);

        buildingState.OnAction(gridPosition);
    }
}
