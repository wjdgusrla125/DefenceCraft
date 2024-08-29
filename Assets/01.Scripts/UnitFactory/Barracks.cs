using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Barracks : Building
{
    public override void EnqueueUnit(UnitType unitType)
    {
        if (unitType == UnitType.Workers || unitType == UnitType.SpearMan || unitType == UnitType.Archers)
        {
            productionQueue.Enqueue(unitType);
            if (!isProducing)
            {
                StartCoroutine(ProduceUnit());
            }
        }
    }
}
