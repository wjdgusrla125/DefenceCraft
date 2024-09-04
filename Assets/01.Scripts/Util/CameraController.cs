using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private float horizontal;
    private float vertical;
    private float scroll;
    private float moveSpeed = 20f;
    private float zoomSpeed = 1000f;
    private float rotationSpeed = 30f;
    private float minZoomDistance = 10f;  // 최소 확대 거리
    private float maxZoomDistance = 45f;  // 최대 확대 거리
    private float minYRotation = 45f;
    private float maxYRotation = 60f;

    private Vector3 dragOrigin;

    private Camera _camera;

    private void Awake()
    {
        _camera = Camera.main;
    }

    private void Update()
    {
        MovementCam();
        ScrollCam();
        RotateCam();
    }

    private void MovementCam()
    {
        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");

        Vector3 forward = transform.forward;
        Vector3 right = transform.right;

        forward.y = 0;
        right.y = 0;

        forward.Normalize();
        right.Normalize();

        Vector3 movement = (forward * vertical + right * horizontal).normalized;

        transform.position += movement * (moveSpeed * Time.deltaTime);
    }

    private void ScrollCam()
    {
        scroll = Input.GetAxis("Mouse ScrollWheel");

        if (scroll != 0.0f)
        {
            Vector3 direction = transform.forward * (scroll * zoomSpeed * Time.deltaTime);
            Vector3 newPosition = transform.position + direction;

            // 카메라의 현재 위치에서 중심점(예: 월드 원점)까지의 거리 계산
            float distance = Vector3.Distance(newPosition, Vector3.zero);

            // 최소 및 최대 거리 내로 이동 범위를 제한
            if (distance >= minZoomDistance && distance <= maxZoomDistance)
            {
                transform.position = newPosition;
            }
        }
    }

    private void RotateCam()
    {
        if (!Input.GetMouseButton(2)) return;

        if (Input.GetMouseButtonDown(2))
        {
            dragOrigin = Input.mousePosition;
            return;
        }

        Vector3 direction = Input.mousePosition - dragOrigin;
        float rotationX = -direction.y * rotationSpeed * Time.deltaTime;
        float rotationY = direction.x * rotationSpeed * Time.deltaTime;

        Vector3 currentRotation = transform.eulerAngles;
        currentRotation.x += rotationX;
        currentRotation.y += rotationY;

        currentRotation.x = Mathf.Clamp(currentRotation.x, minYRotation, maxYRotation);

        transform.eulerAngles = currentRotation;
        dragOrigin = Input.mousePosition;
    }
}
