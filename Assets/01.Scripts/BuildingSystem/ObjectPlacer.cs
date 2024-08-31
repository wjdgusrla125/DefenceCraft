using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPlacer : MonoBehaviour
{
    [SerializeField] private List<GameObject> placedGameObject = new();
    [SerializeField] private GameObject constructPrefab;

    public int PlaceObject(GameObject prefab, Vector3 position, Character builder)
    {
        //StartCoroutine(PlaceObjectCoroutine(prefab, position));

        StartCoroutine(PlaceObjectCoroutine(prefab, position, builder));
        return placedGameObject.Count - 1;
    }

    public void RemoveObjectAt(int gameObjectIndex)
    {
        if (placedGameObject.Count <= gameObjectIndex || placedGameObject[gameObjectIndex] == null) return;
        
        Destroy(placedGameObject[gameObjectIndex]);
        
        placedGameObject[gameObjectIndex] = null;
    }

    private IEnumerator PlaceObjectCoroutine(GameObject prefab, Vector3 position, Character builder)
    {
        // Wait for the builder to reach the construction site
        while (Vector3.Distance(builder.transform.position, position) > 1f)
        {
            yield return null;
        }

        GameObject constructObject = Instantiate(constructPrefab, position, Quaternion.identity);

        // Construction time
        yield return new WaitForSeconds(10f);

        Destroy(constructObject);
        
        GameObject newObject = Instantiate(prefab, position, Quaternion.identity);
        placedGameObject.Add(newObject);

        // Reset the builder's state
        builder.ResetSkillAndMove(builder.transform.position);
    }
    
    
    // private IEnumerator PlaceObjectCoroutine(GameObject prefab, Vector3 position)
    // {
    //     GameObject constructObject = Instantiate(constructPrefab);
    //     constructObject.transform.position = position;
    //
    //     yield return new WaitForSeconds(10f);
    //
    //     Destroy(constructObject);
    //     
    //     GameObject newObject = Instantiate(prefab);
    //     newObject.transform.position = position;
    //     
    //     placedGameObject.Add(newObject);
    // }
}
