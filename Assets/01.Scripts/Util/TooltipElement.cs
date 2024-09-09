using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TooltipElement : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private TooltipWindow tooltipWindow;
    [SerializeField] private string header;
    [SerializeField] private string description;
    [SerializeField] private RectTransform rectTransform;

    public void OnPointerEnter(PointerEventData eventData)
    {
        tooltipWindow.ShowTooltip(header, description, rectTransform.position.x);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        tooltipWindow.HideTooltip();
    }
}
