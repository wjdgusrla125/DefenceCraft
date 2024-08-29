using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    private void Start()
    {
        SelectionManager.Instance.allUnitsList.Add(gameObject);
    }

    private void OnDestroy()
    {
        SelectionManager.Instance.allUnitsList.Remove(gameObject);
    }
}