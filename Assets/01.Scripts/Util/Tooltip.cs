using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Tooltip : MonoBehaviour
{
    public TextMeshProUGUI tooltipText;
    private RectTransform tooltipRectTransform;

    void Start()
    {
        tooltipRectTransform = GetComponent<RectTransform>();
        HideTooltip();
    }

    void Update()
    {
        Vector2 mousePosition = Input.mousePosition;
        tooltipRectTransform.position = mousePosition;
    }

    public void ShowTooltip(string message)
    {
        tooltipText.text = message;
        //gameObject.SetActive(true); 
    }

    public void HideTooltip()
    {
        //gameObject.SetActive(false);
    }
}