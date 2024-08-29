using UnityEngine;

public class FSM_EnemyState_Move : VMyState<FSM_EnemyState>
{
    public override FSM_EnemyState StateEnum => FSM_EnemyState.FSM_EnemyState_Move;
    private EnemyCharacter _enemyCharacter;

    protected override void Awake()
    {
        base.Awake();
        _enemyCharacter = GetComponent<EnemyCharacter>();
    }

    protected override void EnterState()
    {
        _enemyCharacter._animator.CrossFade(EnemyCharacter.EMoveHash, 0.0f);
    }

    protected override void ExcuteState()
    {
        _enemyCharacter.DetectTarget();
    }

    protected override void ExitState()
    {
        
    }
}