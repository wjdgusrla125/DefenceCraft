using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnitUI : MonoBehaviour
{
    public Slider hpbar;
    
    private Transform cam;
    public UnitHealth _Health;

    private void Awake()
    {
        cam = Camera.main.transform;
        _Health = GetComponentInParent<UnitHealth>();
    }

    private void Update()
    {
        UIUPdate();
    }

    private void UIUPdate()
    {
        transform.LookAt(transform.position + cam.rotation * Vector3.forward,cam.rotation*Vector3.up);

        hpbar.value = _Health.currrentUnitHP / _Health.MaxUnitHP;
    }
}
