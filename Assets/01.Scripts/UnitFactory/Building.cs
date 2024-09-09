using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum BuildingType
{
    None,
    Castle,
    Barrack,
    Wall,
    MagicTower
}

public abstract class Building : MonoBehaviour
{
    public Queue<UnitType> productionQueue = new Queue<UnitType>();
    public bool isProducing = false;
    public float productionTime = 5f;
    public float currentProductionTime = 0f;

    public Sprite icon;
    public string Name;
    private UnitHealth _buildingHealth;

    public BuildingType buildingType;
    
    public float constructionTime = 10f;

    private void Start()
    {
        //만약 데미지 안들어올경우 여기 수정
        _buildingHealth = GetComponentInParent<UnitHealth>();
        SelectionManager.Instance.allBuildingList.Add(gameObject);

        SetConstructionTime();
    }

    private void Update()
    {
        DistructBuilding();
    }

    private void OnDestroy()
    {
        SelectionManager.Instance.allBuildingList.Remove(gameObject);
    }

    public virtual void EnqueueUnit(UnitType unitType)
    {
        
    }

    protected IEnumerator ProduceUnit()
    {
        while (productionQueue.Count > 0)
        {
            isProducing = true;
            UnitType unitType = productionQueue.Peek();
            currentProductionTime = 0f;

            while (currentProductionTime < productionTime)
            {
                currentProductionTime += Time.deltaTime;
                yield return null;
            }

            GameObject unit = UnitFactory.CreateUnit(unitType, transform.position + Vector3.forward * 2);
            productionQueue.Dequeue();

            isProducing = false;
            currentProductionTime = 0f;
        }
    }

    private void DistructBuilding()
    {
        if (_buildingHealth.IsDeath())
        {
            Destroy(gameObject);
        }
    }
    
    private void SetConstructionTime()
    {
        switch (buildingType)
        {
            case BuildingType.Castle:
                constructionTime = 20f;
                break;
            case BuildingType.Barrack:
                constructionTime = 15f;
                break;
            case BuildingType.Wall:
                constructionTime = 10f;
                break;
            case BuildingType.MagicTower:
                constructionTime = 18f;
                break;
            default:
                constructionTime = 10f;
                break;
        }
    }
    
    public float GetConstructionTime()
    {
        return constructionTime;
    }
}