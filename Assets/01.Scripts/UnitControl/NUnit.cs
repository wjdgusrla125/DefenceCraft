using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NUnit : MonoBehaviour
{
    public Sprite icon;
    public string Name;
    public int unitCost;
    
    private void Start()
    {
        NSelectionManager.Instance.allUnitsList.Add(gameObject);
    }

    private void OnDestroy()
    {
        NSelectionManager.Instance.allUnitsList.Remove(gameObject);
    }
}