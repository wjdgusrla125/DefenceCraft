using UnityEngine;

[CreateAssetMenu(menuName = "FSM/Conditions/ReachedTarget")]
public class ReachedTargetCondition : TransitionCondition
{
    public override bool Evaluate(Unit unit)
    {
        if (unit.Target == null)
            return false;

        float distance = Vector3.Distance(unit.transform.position, unit.Target.position);
        float attackRange = GetAttackRangeFromUnit(unit);

        return distance <= attackRange;
    }

    private float GetAttackRangeFromUnit(Unit unit)
    {
        foreach (var state in unit.StateMachine.availableStates)
        {
            if (state is MeleeAttackState melee)
                return melee.attackRange;
            if (state is RangedAttackState ranged)
                return ranged.attackRange;
        }
        return 0f;
    }
}