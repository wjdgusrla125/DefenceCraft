using UnityEngine;

public class FSM_EnemyState_Attack : VMyState<FSM_EnemyState>
{
    public override FSM_EnemyState StateEnum => FSM_EnemyState.FSM_EnemyState_Attack;
    private EnemyCharacter _enemyCharacter;
    private float attackCooldown = 1f;
    private float lastAttackTime;

    protected override void Awake()
    {
        base.Awake();
        _enemyCharacter = GetComponent<EnemyCharacter>();
    }

    protected override void EnterState()
    {
        _enemyCharacter._animator.CrossFade(EnemyCharacter.EAttackHash, 0.0f);
    }

    protected override void ExcuteState()
    {
        if (Time.time - lastAttackTime >= attackCooldown)
        {
            lastAttackTime = Time.time;
            Collider[] obstacles = Physics.OverlapSphere(_enemyCharacter.transform.position, _enemyCharacter.attackDistance, _enemyCharacter.obstacleLayer);
            
            if (obstacles.Length > 0)
            {
                Transform nearestObstacle = _enemyCharacter.GetNearestObstacle(obstacles);
            }
            else
            {
                _enemyCharacter.Fsm.ChangeState(FSM_EnemyState.FSM_EnemyState_Move);
            }
        }

        _enemyCharacter.DetectTarget();
    }

    protected override void ExitState()
    {
        
    }
}