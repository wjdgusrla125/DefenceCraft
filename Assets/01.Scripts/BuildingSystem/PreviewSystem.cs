
using UnityEngine;
using UnityEngine.AI;

public class PreviewSystem : MonoBehaviour
{
    [SerializeField]
    private float previewYOffset = 0.06f;

    [SerializeField]
    private GameObject cellIndicator;
    private GameObject previewObject;

    [SerializeField]
    private Material previewMaterialPrefab;
    private Material previewMaterialInstance;

    private Renderer cellIndicatorRenderer;

    private void Start()
    {
        previewMaterialInstance = new Material(previewMaterialPrefab);
        cellIndicator.SetActive(false);
        cellIndicatorRenderer = cellIndicator.GetComponentInChildren<Renderer>();
    }

    public void StartShowingPlacementPreview(GameObject prefab, Vector2Int size, int rotation)
    {
        previewObject = Instantiate(prefab);
        previewObject.GetComponent<NavMeshObstacle>().enabled = false;
        PreparePreview(previewObject);
        PrepareCursor(size, rotation);
        cellIndicator.SetActive(true);
    }

    private void PrepareCursor(Vector2Int size, int rotation)
    {
        if (rotation % 2 != 0)
        {
            size = new Vector2Int(size.y, size.x);
        }
        
        Vector2Int halfSize = size / 2;
        
        if(size.x > 0 || size.y > 0)
        {
            cellIndicator.transform.localScale = new Vector3(halfSize.x, 1, halfSize.y);
            cellIndicatorRenderer.material.mainTextureScale = halfSize;
        }
    }

    private void PreparePreview(GameObject previewObject)
    {
        Renderer[] renderers = previewObject.GetComponentsInChildren<Renderer>();
        
        foreach(Renderer renderer in renderers)
        {
            Material[] materials = renderer.materials;
            
            for (int i = 0; i < materials.Length; i++)
            {
                materials[i] = previewMaterialInstance;
            }
            
            renderer.materials = materials;
        }
    }

    public void StopShowingPreview()
    {
        cellIndicator.SetActive(false );

        if (previewObject != null)
        {
            Destroy(previewObject );
        }
    }

    public void UpdatePosition(Vector3 position, bool validity, int rotation)
    {
        if(previewObject != null)
        {
            MovePreview(position);
            RotatePreview(rotation);
            ApplyFeedbackToPreview(validity);
        }

        MoveCursor(position);
        RotateCursor(rotation);
        ApplyFeedbackToCursor(validity);
    }

    private void ApplyFeedbackToPreview(bool validity)
    {
        Color c = validity ? Color.white : Color.red;
        
        c.a = 0.5f;
        previewMaterialInstance.color = c;
    }

    private void ApplyFeedbackToCursor(bool validity)
    {
        Color c = validity ? Color.white : Color.red;

        c.a = 0.5f;
        cellIndicatorRenderer.material.color = c;
    }

    private void MoveCursor(Vector3 position)
    {
        cellIndicator.transform.position = position;
    }
    
    private void RotateCursor(int rotation)
    {
        cellIndicator.transform.rotation = Quaternion.Euler(0, rotation * 90, 0);
    }
    
    private void MovePreview(Vector3 position)
    {
        previewObject.transform.position = new Vector3(position.x, position.y + previewYOffset, position.z);
    }
    
    private void RotatePreview(int rotation)
    {
        previewObject.transform.rotation = Quaternion.Euler(0, rotation * 90, 0);
    }

    internal void StartShowingRemovePreview()
    {
        cellIndicator.SetActive(true);
        PrepareCursor(Vector2Int.one,0);
        ApplyFeedbackToCursor(false);
    }
}