using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

public class NotifyBase
{
    
}

public interface IVMyState
{
    void OnNotify<T, T2>(T eValue, T2 vValue) where T : Enum where T2 : NotifyBase;
    
    public void EnterStateWrapper();
    
    public void ExcuteStateWrapper();
    
    public void ExitStateWrapper();
    
    public void ExcuteState_FixedUpdateWrapper();
    
    public void ExcuteState_LateUpdateWrapper();
    
    public void AddState<T>(NStateMachine<T> owner, ref object _states) where T : Enum;
}

public abstract class VMyStateBase : MonoBehaviour
{
    
}

public abstract class VMyState<T> : VMyStateBase, IVMyState where T : Enum
{
    public abstract T StateEnum { get; }
    
    [NonSerialized]public NStateMachine<T> OwnerNStateMachine;

    protected virtual void Awake()
    {
        
    }
    
    protected virtual void Start()
    {
        
    }

    public virtual void OnNotify<T1, T2>(T1 eValue, T2 vValue) where T1 : Enum where T2 : NotifyBase
    {
        throw new NotImplementedException();
    }

    public void EnterStateWrapper()
    {
        EnterState();
    }

    public void ExcuteStateWrapper()
    {
        ExcuteState();
    }
    
    public void ExcuteState_FixedUpdateWrapper()
    {
        ExcuteState_FixedUpdate();
    }
    
    public void ExcuteState_LateUpdateWrapper()
    {
        ExcuteState_LateUpdate();
    }

    public virtual void ExitStateWrapper()
    {
        ExitState();
    }

    public void AddState<T1>(NStateMachine<T1> owner, ref object _states) where T1 : Enum
    {
        var cast =_states as Dictionary<T, IVMyState>;
        OwnerNStateMachine = owner as NStateMachine<T>;
        
        if (cast != null)
        {
            if (!cast.ContainsKey(StateEnum))
            {
                cast.Add(StateEnum, this);
            }
            else
            {
                Debug.LogWarning($"State {StateEnum} has already been added to the state machine.");
            }
        }
    }
    
    protected virtual void EnterState()
    {
        
    }

    protected virtual void ExcuteState()
    {
        
    }

    protected virtual void ExitState()
    {
        
    }
    
    protected virtual void ExcuteState_FixedUpdate()
    {
        
    }

    protected virtual void ExcuteState_LateUpdate()
    {
        
    }
}

public abstract class HFSMVMyState<T, T2> : VMyState<T2> where T : Enum where T2 : Enum
{
    [FormerlySerializedAs("HSFM_StateMachine")] public  NStateMachine<T> hsfmNStateMachine;

    public override void ExitStateWrapper()
    {
        base.ExitStateWrapper();
        
        if (hsfmNStateMachine)
        {
            hsfmNStateMachine.ChangeStateNull();
        }
    }
}

public class NStateMachine<T> : MonoBehaviour where T : Enum
{
    [SerializeField] private T defaultState;
    
    [SerializeField] private IVMyState _currentMyState;
    
    private Dictionary<T, IVMyState> _states = new();
    
    public IVMyState GetCurrentState()
    {
        return _currentMyState;
    }
    
    NStateMachine<T> GetSuperOwnerStateMachile()
    {
        NStateMachine<T> nStateMachine = GetComponentInParent< NStateMachine<T>>();
        if (nStateMachine)
        {
            return nStateMachine.GetSuperOwnerStateMachile();
        }

        return this;
    }
    
    private void ChangeState_Internal(IVMyState newMyState)
    {
        if (_currentMyState != null)
        {
            _currentMyState.ExitStateWrapper();
        }

        if (newMyState == null)
        {
            _currentMyState = null;
            return;
        }

        _currentMyState = newMyState;
        _currentMyState.EnterStateWrapper();
    }

    public void OnNotify<T1, T2>(T1 eValue, T2 vValue) where T1 : Enum where T2 : NotifyBase
    {
        _currentMyState.OnNotify(eValue, vValue);
    }

    public void ChangeStateNull()
    {
        ChangeState_Internal(null);
    }

    public void ChangeState(T state)
    {
        if (_states.TryGetValue(state, out var newState))
        {
            ChangeState_Internal(newState);
        }
        else
        {
            Debug.LogError($"State {state} not found in the state machine.");
            ChangeState_Internal(null);
        }
    }
    
    protected virtual void Awake()
    {
        var stateArray = GetComponents<VMyStateBase>().OfType<IVMyState>().ToList();
        foreach (var state in stateArray)
        {
            object states = _states;
            state.AddState(this, ref states);
        }
    }

    protected virtual void Start()
    {
        ChangeState(defaultState);
    }
    
    void Update()
    {
        if (_currentMyState != null)
        {
            _currentMyState.ExcuteStateWrapper();
        }
    }

    private void FixedUpdate()
    {
        if (_currentMyState != null)
        {
            _currentMyState.ExcuteState_FixedUpdateWrapper();
        }
    }

    private void LateUpdate()
    {
        if (_currentMyState != null)
        {
            _currentMyState.ExcuteState_LateUpdateWrapper();
        }
    }
}