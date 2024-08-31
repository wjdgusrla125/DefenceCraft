using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitMana : MonoBehaviour
{
    public float currrentUnitMP;
    public float MaxUnitMP;
    
    private void Awake()
    {
        currrentUnitMP = MaxUnitMP;
        StartCoroutine(RegenerateMP());
    }

    public void UseMP(float mana)
    {
        currrentUnitMP -= mana;

        if (IsEmptyMana())
        {
            currrentUnitMP = 0.0f;
        }
    }

    private IEnumerator RegenerateMP()
    {
        while (true)
        {
            yield return new WaitForSeconds(1f);
            currrentUnitMP += 1f;

            currrentUnitMP = Mathf.Clamp(currrentUnitMP, 0f, MaxUnitMP);
        }
    }

    public bool IsEmptyMana()
    {
        return currrentUnitMP <= 0;
    }

    public bool IsFullMP()
    {
        return currrentUnitMP >= MaxUnitMP;
    }
}