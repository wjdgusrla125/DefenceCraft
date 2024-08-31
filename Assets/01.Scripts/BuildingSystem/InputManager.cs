using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InputManager : MonoBehaviour
{
    [SerializeField] private Camera sceneCamara;

    [SerializeField] private LayerMask placementLayerMask;

    private Vector3 lastPosition;

    public event Action OnClicked, OnExit, OnRotate;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            OnClicked?.Invoke();
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            OnExit?.Invoke();
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            OnRotate?.Invoke();
        }
    }

    public bool IsPointerOverUI() => EventSystem.current.IsPointerOverGameObject();

    public Vector3 GetSelectedMapPosition()
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = sceneCamara.nearClipPlane;

        Ray ray = sceneCamara.ScreenPointToRay(mousePos);
        RaycastHit hit;

        if (Physics.Raycast(ray,out hit, 100, placementLayerMask))
        {
            lastPosition = hit.point;
        }

        return lastPosition;
    }
}
