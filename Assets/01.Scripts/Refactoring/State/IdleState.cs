using UnityEngine;

[CreateAssetMenu(menuName = "FSM/States/Idle")]
public class IdleState : UnitState
{
    public override void Enter(Unit unit)
    {
        unit.Agent.isStopped = true;
    }

    public override void Execute(Unit unit)
    {
        foreach (var transition in transitions)
        {
            if (transition.ShouldTransition(unit) && unit.StateMachine.CanChangeTo(transition.targetState))
            {
                unit.StateMachine.ChangeState(transition.targetState);
                return;
            }
        }
    }

    public override void Exit(Unit unit)
    {
        unit.Agent.isStopped = false;
    }
}