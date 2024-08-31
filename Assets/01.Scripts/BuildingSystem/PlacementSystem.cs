/*using UnityEngine;

public class PlacementSystem : MonoBehaviour
{
    [SerializeField] private GameObject gridVisualization;
    [SerializeField] private InputManager _inputManager;
    [SerializeField] private PreviewSystem _previewSystem;
    [SerializeField] private Grid _grid;
    [SerializeField] private ObjectPlacer _objectPlacer;
    [SerializeField] private ObjectsDatabaseSO database;

    private GridData floorData, furnitureData;
    private Vector3Int lastDetectedPosition = Vector3Int.zero;
    private IBuildingState buildingState;
    private int currentRotation = 0;

    private void Start()
    {
        StopPlacement();
        floorData = new GridData();
        furnitureData = new GridData();
    }

    private void Update()
    {
        if (buildingState == null) return;

        if (Input.GetMouseButtonDown(1))
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
        _inputManager.OnRotate += RotateStructure;
        
        currentRotation = 0;
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
        _inputManager.OnRotate -= RotateStructure;

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

    private void RotateStructure()
    {
        currentRotation = (currentRotation + 90) % 360;
        if (buildingState is PlacementState placementState)
        {
            Vector2Int originalSize = placementState.GetSelectedObjectSize();
            Vector2Int rotatedSize = RotateSize(originalSize);
            
            _previewSystem.RotatePreview(currentRotation);
            _previewSystem.UpdatePreviewSize(rotatedSize);
            placementState.SetRotatedSize(rotatedSize);
            
            // 회전 후 위치 유효성 다시 확인
            Vector3 mousePosition = _inputManager.GetSelectedMapPosition();
            Vector3Int gridPosition = _grid.WorldToCell(mousePosition);
            buildingState.UpdateState(gridPosition);
        }
    }

    private Vector2Int RotateSize(Vector2Int originalSize)
    {
        return currentRotation % 180 == 0 ? originalSize : new Vector2Int(originalSize.y, originalSize.x);
    }
}*/

using UnityEngine;

public class PlacementSystem : MonoBehaviour
{
    [SerializeField] private GameObject gridVisualization;
    [SerializeField] private InputManager _inputManager;
    [SerializeField] private PreviewSystem _previewSystem;
    [SerializeField] private Grid _grid;
    [SerializeField] private ObjectPlacer _objectPlacer;
    [SerializeField] private ObjectsDatabaseSO database;
    [SerializeField] private SelectionManager _selectionManager; // Add this line

    private GridData floorData, furnitureData;
    private Vector3Int lastDetectedPosition = Vector3Int.zero;
    private IBuildingState buildingState;
    private int currentRotation = 0;

    private void Start()
    {
        StopPlacement();
        floorData = new GridData();
        furnitureData = new GridData();
    }

    private void Update()
    {
        if (buildingState == null) return;

        if (Input.GetMouseButtonDown(1))
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

        buildingState = new PlacementState(ID, _grid, _previewSystem, database, floorData, furnitureData, _objectPlacer); // Modified this line

        _inputManager.OnClicked += PlaceStructure;
        _inputManager.OnExit += StopPlacement;
        _inputManager.OnRotate += RotateStructure;
        
        currentRotation = 0;
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
        _inputManager.OnRotate -= RotateStructure;

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

    private void RotateStructure()
    {
        currentRotation = (currentRotation + 90) % 360;
        if (buildingState is PlacementState placementState)
        {
            Vector2Int originalSize = placementState.GetSelectedObjectSize();
            Vector2Int rotatedSize = RotateSize(originalSize);
            
            _previewSystem.RotatePreview(currentRotation);
            _previewSystem.UpdatePreviewSize(rotatedSize);
            placementState.SetRotatedSize(rotatedSize);
            
            // 회전 후 위치 유효성 다시 확인
            Vector3 mousePosition = _inputManager.GetSelectedMapPosition();
            Vector3Int gridPosition = _grid.WorldToCell(mousePosition);
            buildingState.UpdateState(gridPosition);
        }
    }

    private Vector2Int RotateSize(Vector2Int originalSize)
    {
        return currentRotation % 180 == 0 ? originalSize : new Vector2Int(originalSize.y, originalSize.x);
    }
}