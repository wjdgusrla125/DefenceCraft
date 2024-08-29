using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPlacer : MonoBehaviour
{
    [SerializeField] private List<GameObject> placedGameObject = new();

    [SerializeField] private GameObject constructPrefab;

    public int PlaceObject(GameObject prefab, Vector3 position)
    {
        // GameObject newObject = Instantiate(prefab);
        // newObject.transform.position = position;
        //
        // placedGameObject.Add(newObject);

        StartCoroutine(PlaceObjectCoroutine(prefab, position));

        return placedGameObject.Count - 1;
    }

    public void RemoveObjectAt(int gameObjectIndex)
    {
        if (placedGameObject.Count <= gameObjectIndex || placedGameObject[gameObjectIndex] == null) return;
        
        Destroy(placedGameObject[gameObjectIndex]);
        
        placedGameObject[gameObjectIndex] = null;
    }

    private IEnumerator PlaceObjectCoroutine(GameObject prefab, Vector3 position)
    {
        GameObject constructObject = Instantiate(constructPrefab);
        constructObject.transform.position = position;

        yield return new WaitForSeconds(10f);

        Destroy(constructObject);
        
        GameObject newObject = Instantiate(prefab);
        newObject.transform.position = position;
        
        placedGameObject.Add(newObject);
    }
}
