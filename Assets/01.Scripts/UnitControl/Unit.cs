using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Unit : MonoBehaviour
{
    public Sprite icon;
    public string Name;
    public int unitCost;
    
    private void Start()
    {
        SelectionManager.Instance.allUnitsList.Add(gameObject);
    }

    private void OnDestroy()
    {
        SelectionManager.Instance.allUnitsList.Remove(gameObject);
    }
}