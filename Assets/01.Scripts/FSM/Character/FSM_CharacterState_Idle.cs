using UnityEngine;

public class FSM_CharacterState_Idle : VMyState<FSM_CharacterState>
{
    public override FSM_CharacterState StateEnum => FSM_CharacterState.FSM_CharacterState_Idle;
    private Character _character;
    private GameObject target;

    protected override void Awake()
    {
        base.Awake();
        _character = GetComponent<Character>();
    }

    protected override void EnterState()
    {
        _character._animator.CrossFade(Character.IdleHash, 0.0f);
    }

    protected override void ExcuteState()
    {
        if (_character._unitHealth.IsDeath())
        {
            _character.Fsm.ChangeState(FSM_CharacterState.FSM_CharacterState_Dead);
        }
        
        _character.DetectEnemy();
        
        if (Input.GetMouseButtonDown(1) && SelectionManager.Instance.unitSelected.Contains(this.gameObject))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                if (((1 << hit.collider.gameObject.layer) & _character.resourceLayer) != 0)
                {
                    target = hit.collider.gameObject;
                    _character.UseSkillOnResource(target);
                    _character.Fsm.ChangeState(FSM_CharacterState.FSM_CharacterState_Move);
                }
                else
                {
                    _character.Fsm.ChangeState(FSM_CharacterState.FSM_CharacterState_Move);
                }
            }
        }

        if (target != null)
        {
            _character.CheckSkillRangeAndExecute();
        }
        
    }
    
    protected override void ExitState()
    {
        
    }
}