using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FSM_CharacterState_Dead : VMyState<FSM_CharacterState>
{
    public override FSM_CharacterState StateEnum => FSM_CharacterState.FSM_CharacterState_Dead;
    private Character _character;
    private Animator _animator;

    protected override void Awake()
    {
        base.Awake();
        _animator = GetComponent<Animator>();
        _character = GetComponent<Character>();
    }

    protected override void EnterState()
    {
        _animator.CrossFade(Character.DeadHash, 0.0f);
        StartCoroutine(RealDeath());
    }

    protected override void ExcuteState()
    {
        
    }

    protected override void ExitState()
    {
        
    }

    private IEnumerator RealDeath()
    {
        yield return new WaitForSeconds(3f);
        Destroy(this.gameObject);
    }
}
