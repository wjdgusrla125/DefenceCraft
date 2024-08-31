using UnityEngine;

public enum UnitType
{
    Workers,
    SpearMan,
    Archers,
    Mage
}

public static class UnitFactory
{
    public static GameObject CreateUnit(UnitType unitType, Vector3 position)
    {
        GameObject unitPrefab = GetUnitPrefab(unitType);
        if (unitPrefab != null)
        {
            GameObject unit = Object.Instantiate(unitPrefab, position, Quaternion.identity);
            Character character = unit.GetComponent<Character>();
            
            return unit;
        }
        return null;
    }

    public static GameObject GetUnitPrefab(UnitType unitType)
    {
        string prefabPath = "Unit/";
        
        switch (unitType)
        {
            case UnitType.Workers:
                prefabPath += "Worker";
                break;
            case UnitType.Archers:
                prefabPath += "Archer";
                break;
            case UnitType.SpearMan:
                prefabPath += "SpearMan";
                break;
            case UnitType.Mage:
                prefabPath += "Mage";
                break;
            default:
                return null;
        }
        return Resources.Load<GameObject>(prefabPath);
    }
}