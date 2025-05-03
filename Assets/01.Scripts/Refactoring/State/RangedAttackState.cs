using UnityEngine;

[CreateAssetMenu(menuName = "FSM/States/RangedAttack")]
public class RangedAttackState : UnitState
{
    public float attackRange = 6.0f;
    public float attackCooldown = 1.5f;
    public GameObject projectilePrefab;
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
            
            GameObject projectile = Instantiate(projectilePrefab, unit.transform.position + Vector3.up, Quaternion.identity);
            projectile.transform.LookAt(unit.Target.position);
            
            Debug.Log($"{unit.name} 원거리 공격!");
            lastAttackTime = Time.time;
        }
    }

    public override void Exit(Unit unit)
    {
        unit.Agent.isStopped = false;
    }
}