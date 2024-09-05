/*using UnityEngine;

public class FSM_CharacterState_MagicSkill : VMyState<FSM_CharacterState>
{
    public override FSM_CharacterState StateEnum => FSM_CharacterState.FSM_CharacterState_MagicSkill;
    private Character _character;
    private GameObject SpellIndicator;

    protected override void Awake()
    {
        base.Awake();
        _character = GetComponent<Character>();
    }

    protected override void EnterState()
    {
        _character._animator.CrossFade(_character.activeSkillInstance.info.AnimationName_Hash, 0.0f);
        SpellIndicator = Instantiate(_character.MagicCircle,Vector3.zero, Quaternion.Euler(-90,0,0));
    }

    protected override void ExcuteState()
    {
        if (_character._unitHealth.IsDeath())
        {
            _character.Fsm.ChangeState(FSM_CharacterState.FSM_CharacterState_Dead);
        }
        
        SpellIndicator.transform.position = Input.mousePosition;
        
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
        DestroyImmediate(SpellIndicator);
    }
}*/

using UnityEngine;

public class FSM_CharacterState_MagicSkill : VMyState<FSM_CharacterState>
{
    public override FSM_CharacterState StateEnum => FSM_CharacterState.FSM_CharacterState_MagicSkill;
    private Character _character;
    private GameObject SpellIndicator;
    private Camera _mainCamera;

    protected override void Awake()
    {
        base.Awake();
        _character = GetComponent<Character>();
        _mainCamera = Camera.main;
    }

    protected override void EnterState()
    {
        _character._animator.CrossFade(_character.activeSkillInstance.info.AnimationName_Hash, 0.0f);
        SpellIndicator = Instantiate(_character.MagicCircle, Vector3.zero, Quaternion.Euler(-90, 0, 0));
    }

    protected override void ExcuteState()
    {
        if (_character._unitHealth.IsDeath())
        {
            _character.Fsm.ChangeState(FSM_CharacterState.FSM_CharacterState_Dead);
            return;
        }

        UpdateSpellIndicatorPosition();

        if (Input.GetMouseButtonDown(0))
        {
            CastSpell();
        }

        if (Input.GetMouseButtonDown(1))
        {
            _character.Fsm.ChangeState(FSM_CharacterState.FSM_CharacterState_Move);
        }
    }

    private void UpdateSpellIndicatorPosition()
    {
        Ray ray = _mainCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, _character.groundLayer))
        {
            SpellIndicator.transform.position = hit.point + new Vector3(0, (float)0.05, 0);
            SpellIndicator.transform.rotation = Quaternion.FromToRotation(Vector3.up, hit.normal) * Quaternion.Euler(-90, 0, 0);
        }
    }

    private void CastSpell()
    {
        Ray ray = _mainCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, _character.groundLayer))
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

    protected override void ExitState()
    {
        if (SpellIndicator != null)
        {
            Destroy(SpellIndicator);
        }
    }
}