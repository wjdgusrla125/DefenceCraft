using UnityEngine;

public class SelectableUnit : MonoBehaviour
{
    [SerializeField] private GameObject selectImage;
    private Color originalColor;
    
    public bool IsSelected { get; private set; }
    
    public void Select()
    {
        IsSelected = true;
        selectImage.SetActive(true);
    }

    public void Deselect()
    {
        IsSelected = false;
        selectImage.SetActive(false);
    }
}