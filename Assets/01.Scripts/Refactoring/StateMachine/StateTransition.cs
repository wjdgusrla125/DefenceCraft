using UnityEngine;

[System.Serializable]
public class StateTransition
{
    public UnitState targetState;
    public TransitionCondition condition;

    public bool ShouldTransition(Unit unit) => condition != null && condition.Evaluate(unit);
}
