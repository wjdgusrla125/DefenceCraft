using System;
using UnityEngine;

public abstract class TransitionCondition : ScriptableObject
{
    public abstract bool Evaluate(Unit unit);
}
