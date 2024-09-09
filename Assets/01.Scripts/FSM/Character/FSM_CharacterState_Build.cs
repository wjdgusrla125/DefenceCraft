using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FSM_CharacterState_Build : VMyState<FSM_CharacterState>
{
    public override FSM_CharacterState StateEnum => FSM_CharacterState.FSM_CharacterState_Build;
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
        //건물 위치로 이동 후 애니메이션 재생
        _animator.CrossFade(Character.SkillHash, 0.1f);
    }

    protected override void ExcuteState()
    {
        if (_character._unitHealth.IsDeath())
        {
            _character.Fsm.ChangeState(FSM_CharacterState.FSM_CharacterState_Dead);
        }
        
    }

    protected override void ExitState()
    {
        
    }
}
