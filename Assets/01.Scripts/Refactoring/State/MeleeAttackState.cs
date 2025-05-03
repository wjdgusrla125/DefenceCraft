using UnityEngine;

[CreateAssetMenu(menuName = "FSM/States/MeleeAttack")]
public class MeleeAttackState : UnitState
{
    public float attackRange = 1.5f;
    public float attackCooldown = 1.0f;
    private float lastAttackTime;

    public override void Enter(Unit unit)
    {
        lastAttackTime = Time.time - attackCooldown;
        unit.Agent.isStopped = true;
    }

    public override void Execute(Unit unit)
    {
        if (unit.Target == null)
        {
            unit.StateMachine.ChangeState(unit.StateMachine.initialState);
            return;
        }

        float distance = Vector3.Distance(unit.transform.position, unit.Target.position);
        
        if (distance <= attackRange && Time.time >= lastAttackTime + attackCooldown)
        {
            Debug.Log($"{unit.name} 근거리 공격!");
            //데미지 기능 추가
            lastAttackTime = Time.time;
        }
    }

    public override void Exit(Unit unit)
    {
        unit.Agent.isStopped = false;
    }
}