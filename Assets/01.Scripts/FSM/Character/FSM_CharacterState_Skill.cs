using UnityEngine;
using System.Collections;

public class FSM_CharacterState_Skill : VMyState<FSM_CharacterState>
{
    public override FSM_CharacterState StateEnum => FSM_CharacterState.FSM_CharacterState_Skill;
    private Character _character;
    private Animator _animator;
    private Coroutine _resourceIncreaseCoroutine;
    
    protected override void Awake()
    {
        base.Awake();
        _animator = GetComponent<Animator>();
        _character = GetComponent<Character>();
    }

    protected override void EnterState()
    {   
        if (_character == null || _character.activeSkillInstance == null)
        {
            _character?.Fsm.ChangeState(FSM_CharacterState.FSM_CharacterState_Idle);
            return;
        }
        
        if (_character.activeSkillInstance.info.SkillIndex == 0)
        {
            _resourceIncreaseCoroutine = StartCoroutine(IncreaseResourceCoroutine());
        }
        
        _animator.CrossFade(_character.activeSkillInstance.info.AnimationName_Hash, 0.1f);
    }

    protected override void ExcuteState()
    {
        if (_character._unitHealth.IsDeath())
        {
            _character.Fsm.ChangeState(FSM_CharacterState.FSM_CharacterState_Dead);
        }
        
        _character.RotateToTarget(_character.activeSkillInstance.target.transform);
        
        if (Input.GetMouseButtonDown(1))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                if (((1 << hit.collider.gameObject.layer) & _character.resourceLayer) != 0)
                {
                    ChangeTarget(hit.collider.gameObject);
                }
                else if (((1 << hit.collider.gameObject.layer) & _character.groundLayer) != 0)
                {
                    _character.ResetSkillAndMove(hit.point);
                }
            }
        }
        
        
    }

    protected override void ExitState()
    {
        if (_resourceIncreaseCoroutine != null)
        {
            StopCoroutine(_resourceIncreaseCoroutine);
            _resourceIncreaseCoroutine = null;
        }
    }

    private void ChangeTarget(GameObject newTarget)
    {
        _character.SetSkillTarget(newTarget);
        _character.CheckSkillRangeAndExecute();
    }
    
    private IEnumerator IncreaseResourceCoroutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(2f);
            if (_character.resourceVariable != null)
            {
                _character.resourceVariable.ApplyChange(10);
                //Debug.Log($"Resource increased. New value: {_character.resourceVariable.Value}");
            }
        }
    }
}