/*using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Serialization;

public class SelectionManager : MonoBehaviour
{
    public static SelectionManager Instance { get; private set; }

    public List<GameObject> allUnitsList = new List<GameObject>();
    public List<GameObject> unitSelected = new List<GameObject>();

    #region 유닛 생성

    public List<GameObject> allBuildingList = new List<GameObject>();
    public GameObject buildingSelected;

    #endregion

    public LayerMask clickable;
    public LayerMask ground;
    public LayerMask ignoreUI;
    public GameObject groundMarker;

    public LayerMask Buildinglayer;

    private Camera cam;
    public float offsetRadius = 0.5f;
    private List<Vector3> offsets;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    private void Start()
    {
        cam = Camera.main;
        GenerateOffsets();
    }

    private void Update()
    {
        HandleInput();
    }

    private void HandleInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit, Mathf.Infinity, clickable | ignoreUI | Buildinglayer))
            {
                if (hit.collider.gameObject.layer == LayerMask.NameToLayer("UI")) return;
                
                GameObject clickedObject = hit.collider.gameObject;

                if (clickedObject.GetComponent<Building>() != null)
                {
                    BuildingSelect(clickedObject);
                }
                else if (clickedObject.GetComponent<Character>() != null)
                {
                    if (Input.GetKey(KeyCode.LeftShift))
                    {
                        MultipleSelect(clickedObject);
                    }
                    else
                    {
                        SingleSelect(clickedObject);
                    }
                }
                else
                {
                    if (!Input.GetKey(KeyCode.LeftShift))
                    {
                        DeselectAll();
                    }
                }
            }
            else
            {
                if (!IsPointerOverUIElement())
                {
                    DeselectAll();
                }
            }
        }

        if (Input.GetMouseButtonDown(1) && unitSelected.Count > 0)
        {
            RaycastHit hit;
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit, Mathf.Infinity, ground))
            {
                groundMarker.transform.position = hit.point;
                groundMarker.SetActive(false);
                groundMarker.SetActive(true);

                MoveSelectedUnits(hit.point);
            }
        }   
    }

    private void SingleSelect(GameObject unit)
    {
        DeselectAll();
        unitSelected.Add(unit);
        SelectUnit(unit, true);
        UpdateUI();
    }

    private void BuildingSelect(GameObject building)
    {
        DeselectAll();
        buildingSelected = building;
        SelectUnit(building, true);
        UIManager.Instance.ShowBuildingUI(building.GetComponent<Building>());
    }

    private void MultipleSelect(GameObject unit)
    {
        if (!unitSelected.Contains(unit))
        {
            unitSelected.Add(unit);
            SelectUnit(unit, true);
        }
        else
        {
            SelectUnit(unit, false);
            unitSelected.Remove(unit);
        }
        UpdateUI();
    }

    public void DragSelect(GameObject unit)
    {
        if (!unitSelected.Contains(unit))
        {
            unitSelected.Add(unit);
            SelectUnit(unit, true);
            UpdateUI();
        }
    }

    public void DeselectAll()
    {
        foreach (var unit in unitSelected)
        {
            SelectUnit(unit, false);
        }

        if (buildingSelected != null)
        {
            SelectUnit(buildingSelected, false);
            buildingSelected = null;
        }

        groundMarker.SetActive(false);
        unitSelected.Clear();
        UIManager.Instance.HideBuildingUI();
        UIManager.Instance.DisableUnitUI();
    }

    private void SelectUnit(GameObject unit, bool isSelected)
    {
        unit.transform.GetChild(0).gameObject.SetActive(isSelected);
    }

    private void GenerateOffsets()
    {
        offsets = new List<Vector3>();
        int maxUnits = 100;
        for (int i = 0; i < maxUnits; i++)
        {
            float angle = i * Mathf.PI * 2 / maxUnits;
            float radius = offsetRadius * Mathf.Sqrt((float)i / maxUnits);
            float x = Mathf.Cos(angle) * radius;
            float z = Mathf.Sin(angle) * radius;
            offsets.Add(new Vector3(x, 0, z));
        }
    }

    private void MoveSelectedUnits(Vector3 targetPosition)
    {
        Vector3 groupCenter = CalculateGroupCenter();

        for (int i = 0; i < unitSelected.Count; i++)
        {
            Vector3 relativePosition = unitSelected[i].transform.position - groupCenter;
            Vector3 offsetPosition = targetPosition + relativePosition;

            Character character = unitSelected[i].GetComponent<Character>();
            if (character != null)
            {
                NavMeshHit hit;
                if (NavMesh.SamplePosition(offsetPosition, out hit, 1.0f, NavMesh.AllAreas))
                {
                    character.SetMoveTarget(hit.position);
                }
                else
                {
                    character.SetMoveTarget(offsetPosition);
                }
            }
        }
    }

    private Vector3 CalculateGroupCenter()
    {
        Vector3 center = Vector3.zero;
        foreach (var unit in unitSelected)
        {
            center += unit.transform.position;
        }
        return center / unitSelected.Count;
    }

    private void UpdateUI()
    {
        if (UIManager.Instance != null)
        {
            UIManager.Instance.UpdateButtons(unitSelected);
        }
    }
    
    private bool IsPointerOverUIElement()
    {
        return UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject();
    }
}*/

using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class SelectionManager : MonoBehaviour
{
    public static SelectionManager Instance { get; private set; }

    public List<GameObject> allUnitsList = new List<GameObject>();
    public List<GameObject> unitSelected = new List<GameObject>();

    public List<GameObject> allBuildingList = new List<GameObject>();
    public GameObject buildingSelected;

    public LayerMask clickable;
    public LayerMask ground;
    public LayerMask ignoreUI;
    public LayerMask buildingLayer;
    public GameObject groundMarker;
    
    private LayerMask allDetectableLayers;

    private Camera cam;
    public float offsetRadius = 0.5f;
    private List<Vector3> offsets;
    
    private Vector3 rayStart;
    private Vector3 rayEnd;
    public float rayDuration = 0.2f;
    private float rayTimer;
    public Color rayColor = Color.red;
    

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
        
        cam = Camera.main;
    }

    private void Start()
    {
        GenerateOffsets();
        
        allDetectableLayers = clickable | ignoreUI | buildingLayer;
    }

    private void Update()
    {
        HandleInput();
        UpdateRayVisualization();
    }

    private void HandleInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (IsAnyUnitInMagicSkillState())
            {
                return;
            }
            
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            
            rayStart = ray.origin;
            rayEnd = ray.origin + ray.direction * 100f;
            rayTimer = rayDuration;
            
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, allDetectableLayers))
            {
                GameObject clickedObject = hit.collider.gameObject;
                
                if (clickedObject.GetComponent<Building>() != null)
                {
                    BuildingSelect(clickedObject);
                }
                else if (clickedObject.GetComponent<Character>() != null)
                {
                    if (Input.GetKey(KeyCode.LeftShift))
                    {
                        MultipleSelect(clickedObject);
                    }
                    else
                    {
                        SingleSelect(clickedObject);
                    }
                }
                else
                {
                    if (!Input.GetKey(KeyCode.LeftShift))
                    {
                        DeselectAll();
                    }
                }
            }
            else
            {
                if (!IsPointerOverUIElement())
                {
                    DeselectAll();
                }
            }
        }

        if (Input.GetMouseButtonDown(1) && unitSelected.Count > 0)
        {
            RaycastHit hit;
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit, Mathf.Infinity, ground))
            {
                groundMarker.transform.position = hit.point;
                groundMarker.SetActive(false);
                groundMarker.SetActive(true);

                MoveSelectedUnits(hit.point);
            }
        }   
    }
    
    private bool IsAnyUnitInMagicSkillState()
    {
        return unitSelected.Any(unit =>
        {
            Character character = unit.GetComponent<Character>();
            if (character != null && character.Fsm != null)
            {
                var currentState = character.Fsm.GetCurrentState();
                return currentState is FSM_CharacterState_MagicSkill;
            }
            return false;
        });
    }

    private void SingleSelect(GameObject unit)
    {
        DeselectAll();
        unitSelected.Add(unit);
        SelectUnit(unit, true);
        UpdateUI();
    }

    private void BuildingSelect(GameObject building)
    {
        DeselectAll();
        buildingSelected = building;
        SelectUnit(building, true);
        UIManager.Instance.ShowBuildingUI(building.GetComponent<Building>());
    }

    private void MultipleSelect(GameObject unit)
    {
        if (!unitSelected.Contains(unit))
        {
            unitSelected.Add(unit);
            SelectUnit(unit, true);
        }
        else
        {
            SelectUnit(unit, false);
            unitSelected.Remove(unit);
        }
        UpdateUI();
    }

    public void DragSelect(GameObject unit)
    {
        if (!unitSelected.Contains(unit))
        {
            unitSelected.Add(unit);
            SelectUnit(unit, true);
            UpdateUI();
        }
    }

    public void DeselectAll()
    {
        foreach (var unit in unitSelected)
        {
            SelectUnit(unit, false);
        }

        if (buildingSelected != null)
        {
            SelectUnit(buildingSelected, false);
            buildingSelected = null;
        }

        groundMarker.SetActive(false);
        unitSelected.Clear();
        UIManager.Instance.HideBuildingUI();
        UIManager.Instance.DisableAllUI();
    }

    private void SelectUnit(GameObject unit, bool isSelected)
    {
        if (unit != null && unit.transform.childCount > 0)
        {
            unit.transform.GetChild(0).gameObject.SetActive(isSelected);
        }
    }

    private void GenerateOffsets()
    {
        offsets = new List<Vector3>();
        int maxUnits = 100;
        for (int i = 0; i < maxUnits; i++)
        {
            float angle = i * Mathf.PI * 2 / maxUnits;
            float radius = offsetRadius * Mathf.Sqrt((float)i / maxUnits);
            float x = Mathf.Cos(angle) * radius;
            float z = Mathf.Sin(angle) * radius;
            offsets.Add(new Vector3(x, 0, z));
        }
    }

    private void MoveSelectedUnits(Vector3 targetPosition)
    {
        Vector3 groupCenter = CalculateGroupCenter();

        for (int i = 0; i < unitSelected.Count; i++)
        {
            Vector3 relativePosition = unitSelected[i].transform.position - groupCenter;
            Vector3 offsetPosition = targetPosition + relativePosition;

            Character character = unitSelected[i].GetComponent<Character>();
            if (character != null)
            {
                NavMeshHit hit;
                if (NavMesh.SamplePosition(offsetPosition, out hit, 1.0f, NavMesh.AllAreas))
                {
                    character.SetMoveTarget(hit.position);
                }
                else
                {
                    character.SetMoveTarget(offsetPosition);
                }
            }
        }
    }

    private Vector3 CalculateGroupCenter()
    {
        Vector3 center = Vector3.zero;
        foreach (var unit in unitSelected)
        {
            center += unit.transform.position;
        }
        return center / unitSelected.Count;
    }

    private void UpdateUI()
    {
        if (UIManager.Instance != null)
        {
            UIManager.Instance.UpdateButtons(unitSelected);
        }
    }
    
    private bool IsPointerOverUIElement()
    {
        return UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject();
    }
    
    
    private void UpdateRayVisualization()
    {
        if (rayTimer > 0)
        {
            rayTimer -= Time.deltaTime;
        }
    }

    private void OnDrawGizmos()
    {
        if (rayTimer > 0)
        {
            Gizmos.color = rayColor;
            Gizmos.DrawLine(rayStart, rayEnd);
        }
    }
}