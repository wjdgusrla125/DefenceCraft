using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridData
{
    private Dictionary<Vector3Int, PlacementData> placedObjects = new();

    public void AddObjectAt(Vector3Int gridPosition, Vector2Int objectSize, int ID, int placedObjectIndex)
    {
        List<Vector3Int> positionToOccupy = CalculatePositions(gridPosition, objectSize);
        PlacementData data = new PlacementData(positionToOccupy, ID, placedObjectIndex);

        foreach (var pos in positionToOccupy)
        {
            if (placedObjects.ContainsKey(pos)) 
                throw new Exception($"Dictionary already contains this cell position {pos}");

            placedObjects[pos] = data;
        }
    }

    private List<Vector3Int> CalculatePositions(Vector3Int gridPosition, Vector2Int objectSize)
    {
        List<Vector3Int> returnVal = new();

        for (int x = -(objectSize.x/2); x < objectSize.x/2; x++)
        {
            for (int y = -(objectSize.y/2); y < objectSize.y/2; y++)
            {
                returnVal.Add(gridPosition + new Vector3Int(x, 0, y));
            }
        }
        
        return returnVal;
    }

    public bool CanPlacedObjectAt(Vector3Int gridPosition, Vector2Int objectSize)
    {
        List<Vector3Int> positionToOccupy = CalculatePositions(gridPosition, objectSize);

        foreach (var pos in positionToOccupy)
        {
            if (placedObjects.ContainsKey(pos)) return false;
        }

        return true;
    }

    public int GetRepresentationIndex(Vector3Int gridPosition)
    {
        if (placedObjects.ContainsKey(gridPosition) == false) 
            return -1;

        return placedObjects[gridPosition].PlacedObjectIndex;
    }

    public void RemoveObjectAt(Vector3Int gridPosition)
    {
        foreach (var pos in placedObjects[gridPosition].occupiedPositions)
        {
            placedObjects.Remove(pos);
        }
    }
}

public class PlacementData
{
    public List<Vector3Int> occupiedPositions;
    
    public int ID { get; private set; }
    
    public int PlacedObjectIndex { get; private set; }

    public PlacementData(List<Vector3Int> _occupiedPositions, int id, int _placedObjectIndex)
    {
        this.occupiedPositions = _occupiedPositions;
        ID = id;
        PlacedObjectIndex = _placedObjectIndex;
    }
}
