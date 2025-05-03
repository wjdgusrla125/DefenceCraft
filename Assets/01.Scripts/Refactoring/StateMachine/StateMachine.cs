using System;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine : MonoBehaviour
{
    public Unit unit;
    public UnitState initialState;
    [SerializeField] private UnitState currentState;
    
    public List<UnitState> availableStates;
    
    void Start()
    {
        ChangeState(initialState);
    }

    void Update()
    {
        currentState?.Execute(unit);
    }

    public void ChangeState(UnitState newState)
    {
        if (!availableStates.Contains(newState)) return;
        
        currentState?.Exit(unit);
        currentState = newState;
        currentState?.Enter(unit);
    }
    
    public bool CanChangeTo(UnitState state) => availableStates.Contains(state);
}