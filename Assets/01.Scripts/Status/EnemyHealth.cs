using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public float currrentUnitHP;
    public float MaxUnitHP;
    
    private void Awake()
    {
        currrentUnitHP = MaxUnitHP;
    }

    public void HPTakeDamage(float damage)
    {
        currrentUnitHP -= damage;

        if (IsDeath())
        {
            currrentUnitHP = 0.0f;
        }
    }

    public void RecoveryHP(float hp)
    {
        currrentUnitHP += hp;

        if (IsFullHP())
        {
            currrentUnitHP = MaxUnitHP;
        }
        
    }

    public bool IsDeath()
    {
        return currrentUnitHP <= 0;
    }

    public bool IsFullHP()
    {
        return currrentUnitHP >= MaxUnitHP;
    }
}
