using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FSM_EnemyState_Idle : VMyState<FSM_EnemyState>
{
    public override FSM_EnemyState StateEnum => FSM_EnemyState.FSM_EnemyState_Idle;
    private EnemyCharacter _enemyCharacter;

    protected override void Awake()
    {
        base.Awake();
    }

    protected override void EnterState()
    {
        _enemyCharacter._animator.CrossFade(EnemyCharacter.EIdleHash,0.0f);
    }

    protected override void ExcuteState()
    {
        
    }

    protected override void ExitState()
    {
        
    }
}
