using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillIndicator : MonoBehaviour
{
    //private float previewYOffset = 0.06f;
    
    [SerializeField] 
    private GameObject skillIndicator;
    private SpriteRenderer skillIndicatorRenderer;

    private void Start()
    {
        skillIndicator.SetActive(false);
        skillIndicatorRenderer = skillIndicator.GetComponentInChildren<SpriteRenderer>();
    }
}