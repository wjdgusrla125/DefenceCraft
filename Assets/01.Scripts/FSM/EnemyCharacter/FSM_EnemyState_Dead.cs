using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FSM_EnemyState_Dead : VMyState<FSM_EnemyState>
{
    public override FSM_EnemyState StateEnum => FSM_EnemyState.FSM_EnemyState_Dead;

    private EnemyCharacter _enemyCharacter;

    protected override void Awake()
    {
        base.Awake();
        _enemyCharacter = GetComponent<EnemyCharacter>();
        
    }
    
    protected override void EnterState()
    {
        _enemyCharacter._animator.CrossFade(EnemyCharacter.EDeadHash,0.0f);
        StartCoroutine(RealDeath());
    }

    protected override void ExcuteState()
    {
        
    }

    protected override void ExitState()
    {
        
    }
    
    private IEnumerator RealDeath()
    {
        yield return new WaitForSeconds(3f);
        Destroy(this.gameObject);
    }
}
