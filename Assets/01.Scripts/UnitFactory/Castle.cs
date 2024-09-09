using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Castle : Building
{
    public override void EnqueueUnit(UnitType unitType)
    {
        if (unitType == UnitType.Workers)
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
        }
    }
}
