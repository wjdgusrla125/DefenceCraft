using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Barracks : Building
{
    public override void EnqueueUnit(UnitType unitType)
    {
        if (unitType == UnitType.Workers || unitType == UnitType.SpearMan || unitType == UnitType.Archers)
        {
            GameObject unitPrefab = UnitFactory.GetUnitPrefab(unitType);
            Unit unitComponent = unitPrefab.GetComponent<Unit>();

            if (GameManager.Instance.Gold.Value >= unitComponent.unitCost)
            {
                GameManager.Instance.Gold.ApplyChange(-1 * unitComponent.unitCost);
                productionQueue.Enqueue(unitType);
                
                if (!isProducing)
                {
                    StartCoroutine(ProduceUnit());
                }
            }
            else
            {
                Debug.Log($"Not enough gold to produce {unitType}. Required: {unitComponent.unitCost}, Available: {GameManager.Instance.Gold.Value}");
            }
        }
    }
}
