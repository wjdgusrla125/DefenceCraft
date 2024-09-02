using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridData
{
    Dictionary<Vector3Int, PlacementData> placedObjects = new();

    public void AddObjectAt(Vector3Int gridPosition, Vector2Int objectSize, int ID, int placedObjectIndex, int rotation)
    {
        List<Vector3Int> positionToOccupy = CalculatePositions(gridPosition, objectSize, rotation);
        
        PlacementData data = new PlacementData(positionToOccupy, ID, placedObjectIndex, rotation);
        
        foreach (var pos in positionToOccupy)
        {
            if (placedObjects.ContainsKey(pos))
                throw new Exception($"Dictionary already contains this cell position {pos}");
            placedObjects[pos] = data;
        }
    }

    private List<Vector3Int> CalculatePositions(Vector3Int gridPosition, Vector2Int objectSize, int rotation)
    {
        //실제 건물의 위치와 그리드의 위치가 다를 때 수정하는 부분
        List<Vector3Int> returnVal = new();
        
        if (rotation % 2 != 0)
        {
            objectSize = new Vector2Int(objectSize.y, objectSize.x);
        }
        
        Vector2Int halfSize = objectSize / 2;
        
        //Debug.Log($"{halfSize.x}, {halfSize.y}");
        
        for (int x = -halfSize.x; x < halfSize.x; x++)
        {
            for (int y = -halfSize.y; y < halfSize.y; y++)
            {
                returnVal.Add(gridPosition + new Vector3Int(x, 0, y));
            }
        }
        return returnVal;
    }

    public bool CanPlaceObejctAt(Vector3Int gridPosition, Vector2Int objectSize, int rotation)
    {
        List<Vector3Int> positionToOccupy = CalculatePositions(gridPosition, objectSize, rotation);
        
        foreach (var pos in positionToOccupy)
        {
            if (placedObjects.ContainsKey(pos)) return false;
        }
        return true;
    }

    internal int GetRepresentationIndex(Vector3Int gridPosition)
    {
        if (placedObjects.ContainsKey(gridPosition) == false) return -1;
        
        return placedObjects[gridPosition].PlacedObjectIndex;
    }

    internal void RemoveObjectAt(Vector3Int gridPosition)
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
    public int Rotation { get; private set; }

    public PlacementData(List<Vector3Int> occupiedPositions, int iD, int placedObjectIndex, int rotation)
    {
        this.occupiedPositions = occupiedPositions;
        ID = iD;
        PlacedObjectIndex = placedObjectIndex;
        Rotation = rotation;
    }
}