using System.Collections.Generic;
using UnityEngine;

public abstract class UnitState : ScriptableObject
{
    public List<StateTransition> transitions;

    public abstract void Enter(Unit unit);
    public abstract void Execute(Unit unit);
    public abstract void Exit(Unit unit);
}
