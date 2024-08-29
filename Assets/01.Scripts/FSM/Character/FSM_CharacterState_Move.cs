using UnityEngine;
using UnityEngine.AI;

public class FSM_CharacterState_Move : VMyState<FSM_CharacterState>
{
    public override FSM_CharacterState StateEnum => FSM_CharacterState.FSM_CharacterState_Move;
    private Character _character;
    private NavMeshAgent _agent;

    protected override void Awake()
    {
        base.Awake();
        _character = GetComponent<Character>();
        _agent = GetComponent<NavMeshAgent>();
    }

    protected override void EnterState()
    {
        _character._animator.CrossFade(Character.MoveHash, 0.1f);

        if (_character.activeSkillInstance != null && _character.activeSkillInstance.target != null)
        {
            _character.SetDestination(_character.activeSkillInstance.target.transform.position, true);
        }
        else
        {
            _character.SetDestination(_character.GetMoveTarget());
        }
    }

    protected override void ExcuteState()
    {
        if (_character._unitHealth.IsDeath())
        {
            _character.Fsm.ChangeState(FSM_CharacterState.FSM_CharacterState_Dead);
        }
        
        _character.DetectEnemy();

        if (_character.activeSkillInstance != null && _character.activeSkillInstance.target != null)
        {
            if (_character.IsWithinSkillRange(_character.activeSkillInstance.target, _character.activeSkillInstance.info.AttackDistance))
            {
                _character.Fsm.ChangeState(FSM_CharacterState.FSM_CharacterState_Skill);
            }
            else
            {
                _character.SetDestination(_character.activeSkillInstance.target.transform.position, true);
            }
        }
        else
        {
            _character.CheckArrivalAndHandleState();
        }
    }

    protected override void ExitState()
    {
        _agent.ResetPath();
    }
}