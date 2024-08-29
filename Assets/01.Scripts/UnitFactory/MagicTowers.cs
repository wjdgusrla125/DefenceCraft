using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicTowers : Building
{
    public override void EnqueueUnit(UnitType unitType)
    {
        if (unitType == UnitType.Mage)
        {
            productionQueue.Enqueue(unitType);
            if (!isProducing)
            {
                StartCoroutine(ProduceUnit());
            }
        }
    }
}
