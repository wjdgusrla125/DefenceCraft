using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Building : MonoBehaviour
{
    protected Queue<UnitType> productionQueue = new Queue<UnitType>();
    protected bool isProducing = false;
    public float productionTime = 5f; // 기본 생산 시간

    private void Start()
    {
        SelectionManager.Instance.allBuildingList.Add(gameObject);
    }

    private void OnDestroy()
    {
        SelectionManager.Instance.allBuildingList.Remove(gameObject);
    }

    public abstract void EnqueueUnit(UnitType unitType);

    protected IEnumerator ProduceUnit()
    {
        while (productionQueue.Count > 0)
        {
            isProducing = true;
            yield return new WaitForSeconds(productionTime);

            UnitType unitType = productionQueue.Dequeue();
            GameObject unit = UnitFactory.CreateUnit(unitType, transform.position + Vector3.forward * 2);
            
            // if (unit != null)
            // {
            //     SelectionManager.Instance.allUnitsList.Add(unit);
            // }

            isProducing = false;
        }
    }
}