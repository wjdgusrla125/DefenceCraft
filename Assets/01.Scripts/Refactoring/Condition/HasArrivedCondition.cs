using UnityEngine;

[CreateAssetMenu(menuName = "FSM/Conditions/HasArrived")]
public class HasArrivedCondition : TransitionCondition
{
    public override bool Evaluate(Unit unit)
    {
        return !unit.Agent.pathPending && unit.Agent.remainingDistance < 0.1f;
    }
}