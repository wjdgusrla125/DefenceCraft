using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SelectionManager : SceneSingleton<SelectionManager>
{
    public RectTransform selectionBoxUI;
    public Canvas canvas;

    private Vector2 startPos;
    private List<SelectableUnit> selectedUnits = new();
    private Dictionary<int, List<SelectableUnit>> unitGroups = new();

    void Update()
    {
        HandleClickSelection();
        HandleBoxSelection();
        HandleGroupKeys();
    }

    void HandleClickSelection()
    {
        if (EventSystem.current.IsPointerOverGameObject())
            return;
        
        if (Input.GetMouseButtonDown(0) && !Input.GetKey(KeyCode.LeftShift))
            ClearSelection();

        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                var unit = hit.collider.GetComponent<SelectableUnit>();
                if (unit != null)
                {
                    if (selectedUnits.Contains(unit))
                    {
                        if (Input.GetKey(KeyCode.LeftShift))
                        {
                            DeselectUnit(unit);
                        }
                    }
                    else
                    {
                        SelectUnit(unit);
                    }
                }
            }
        }
    }

    void HandleBoxSelection()
    {
        if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject())
            return;
        
        if (Input.GetMouseButtonDown(0))
        {
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                canvas.transform as RectTransform,
                Input.mousePosition,
                canvas.worldCamera,
                out startPos
            );
            selectionBoxUI.gameObject.SetActive(true);
        }

        if (Input.GetMouseButton(0))
        {
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                canvas.transform as RectTransform,
                Input.mousePosition,
                canvas.worldCamera,
                out Vector2 currentPos
            );
            UpdateSelectionBox(startPos, currentPos);
        }

        if (Input.GetMouseButtonUp(0))
        {
            selectionBoxUI.gameObject.SetActive(false);
            Vector2 min = selectionBoxUI.anchoredPosition - (selectionBoxUI.sizeDelta / 2);
            Vector2 max = selectionBoxUI.anchoredPosition + (selectionBoxUI.sizeDelta / 2);

            foreach (var unit in FindObjectsOfType<SelectableUnit>())
            {
                Vector2 screenPos = Camera.main.WorldToScreenPoint(unit.transform.position);
                RectTransformUtility.ScreenPointToLocalPointInRectangle(
                    canvas.transform as RectTransform,
                    screenPos,
                    canvas.worldCamera,
                    out Vector2 localPos
                );

                if (localPos.x >= min.x && localPos.x <= max.x && localPos.y >= min.y && localPos.y <= max.y)
                {
                    if (!selectedUnits.Contains(unit))
                        SelectUnit(unit);
                }
            }
        }
    }
    
    void HandleGroupKeys()
    {
        for (int i = 0; i <= 9; i++)
        {
            KeyCode numberKey = KeyCode.Alpha0 + i;

            if (Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(numberKey))
            {
                unitGroups[i] = new List<SelectableUnit>(selectedUnits);
                Debug.Log($"그룹 {i} 저장");
            }
            else if (Input.GetKeyDown(numberKey))
            {
                ClearSelection();
                if (unitGroups.ContainsKey(i))
                {
                    foreach (var unit in unitGroups[i])
                    {
                        if (unit != null) SelectUnit(unit);
                    }
                    Debug.Log($"그룹 {i} 로드");
                }
            }
        }
    }

    void UpdateSelectionBox(Vector2 start, Vector2 end)
    {
        Vector2 size = end - start;
        selectionBoxUI.anchoredPosition = start + size / 2;
        selectionBoxUI.sizeDelta = new Vector2(Mathf.Abs(size.x), Mathf.Abs(size.y));
    }

    void SelectUnit(SelectableUnit unit)
    {
        selectedUnits.Add(unit);
        unit.Select();
    }

    void DeselectUnit(SelectableUnit unit)
    {
        selectedUnits.Remove(unit);
        unit.Deselect();
    }

    void ClearSelection()
    {
        foreach (var unit in selectedUnits)
            unit.Deselect();
        selectedUnits.Clear();
    }
    
    public List<SelectableUnit> GetSelectedUnits()
    {
        return new List<SelectableUnit>(selectedUnits);
    }
}