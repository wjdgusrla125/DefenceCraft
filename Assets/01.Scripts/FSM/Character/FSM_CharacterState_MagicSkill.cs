using UnityEngine;

public class FSM_CharacterState_MagicSkill : VMyState<FSM_CharacterState>
{
    public override FSM_CharacterState StateEnum => FSM_CharacterState.FSM_CharacterState_MagicSkill;
    private Character _character;

    protected override void Awake()
    {
        base.Awake();
        _character = GetComponent<Character>();
    }

    protected override void EnterState()
    {
        _character._animator.CrossFade(_character.activeSkillInstance.info.AnimationName_Hash, 0.0f);
    }

    protected override void ExcuteState()
    {
        if (_character._unitHealth.IsDeath())
        {
            _character.Fsm.ChangeState(FSM_CharacterState.FSM_CharacterState_Dead);
        }
        
        if (Input.GetMouseButtonDown(0))
        {
            
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            
            if (Physics.Raycast(ray, out hit))
            {
                Vector3 targetPosition = hit.point;
                
                foreach (var selectedUnit in SelectionManager.Instance.unitSelected)
                {
                    Character selectedCharacter = selectedUnit.GetComponent<Character>();
                    
                    if (selectedCharacter != null && selectedCharacter.UnitType == _character.UnitType)
                    {
                        selectedCharacter.CastMagicSkill(targetPosition);
                    }
                }
            }
        }

        if (Input.GetMouseButtonDown(1))
        {
            _character.Fsm.ChangeState(FSM_CharacterState.FSM_CharacterState_Move);
        }
    }

    protected override void ExitState()
    {
        
    }
}