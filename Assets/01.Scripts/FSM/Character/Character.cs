using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.AI;

public class Character : CharacterBase<FSM_Character>
{
    [Header("Character")]
    [NonSerialized] public Animator _animator;
    public LayerMask resourceLayer;
    public LayerMask groundLayer;
    
    public List<SkillInfo> SkillInfos;
    public List<SkillInstance> skillInstances;
    public SkillInstance activeSkillInstance;
    
    private Vector3 moveTarget;
    private NavMeshAgent _agent;
    public UnitHealth _unitHealth;
    public UnitMana _unitMana;
    
    public IntVariable resourceVariable;
    public GameObject MagicCircle;
    
    public static readonly int IdleHash = Animator.StringToHash("Idle");
    public static readonly int MoveHash = Animator.StringToHash("Move");
    public static readonly int DeadHash = Animator.StringToHash("Dead");
    
    public int UnitType
    {
        get 
        {
            if (SkillInfos != null && SkillInfos.Count > 0)
            {
                return SkillInfos[0].SkillIndex;
            }
            return -1;
        }
    }

    protected override void Awake()
    {
        base.Awake();
        
        ManageSkills(true);
        
        _animator = GetComponent<Animator>();
        _agent = GetComponent<NavMeshAgent>();
        _unitHealth = GetComponent<UnitHealth>();
        _unitMana = GetComponent<UnitMana>();
        
        _agent.acceleration = 8;
        _agent.angularSpeed = 360;
        _agent.stoppingDistance = 0.1f;
        _agent.radius = 0.2f;
    }

    private void Start()
    {
        UIManager.Instance.OnSkillButtonClicked += HandleSkillButtonClicked;
    }

    private void OnDestroy()
    {
        
        UIManager.Instance.OnSkillButtonClicked -= HandleSkillButtonClicked;
    }
    
    // 스킬 관리
    public void ManageSkills(bool isInitialize = false)
    {
        skillInstances.Clear();
        
        foreach (var skillInfo in SkillInfos)
        {
            SkillInstance inst;
            if (isInitialize)
            {
                inst = gameObject.AddComponent<SkillInstance>();
            }
            else
            {
                inst = gameObject.GetComponent<SkillInstance>();
            }
            inst.info = skillInfo;
            skillInstances.Add(inst);
        }
    }

    public void ExecuteActiveSkill()
    {
        if (activeSkillInstance != null)
        {
            _animator.CrossFade(activeSkillInstance.info.AnimationName_Hash, 0.0f);
            activeSkillInstance.ExecuteSkill();
            Fsm.ChangeState(FSM_CharacterState.FSM_CharacterState_Skill);
        }
    }
    
    public void UseSkillOnResource(GameObject target)
    {
        ManageSkills();
        
        if (skillInstances.Count > 0)
        {
            activeSkillInstance = skillInstances[0];
            activeSkillInstance.target = target;
        }
    }

    // 이동 관리
    public void SetDestination(Vector3 target, bool isSkillTarget = false)
    {
        if (isSkillTarget && activeSkillInstance != null && activeSkillInstance.target != null)
        {
            Vector3 targetPosition = activeSkillInstance.target.transform.position;
            Vector3 directionToTarget = (targetPosition - transform.position).normalized;
            Vector3 destination = targetPosition - directionToTarget * (activeSkillInstance.info.AttackDistance - 0.1f);
            _agent.SetDestination(destination);
        }
        else
        {
            _agent.SetDestination(target);
        }
        moveTarget = target;    
    }

    public Vector3 GetMoveTarget()
    {
        return moveTarget;
    }

    // 거리 계산 및 확인
    public float CalculateDistanceToTarget(GameObject target)
    {
        return Vector3.Distance(transform.position, target.transform.position);
    }

    public bool IsWithinSkillRange(GameObject target, float skillRange)
    {
        return CalculateDistanceToTarget(target) <= skillRange;
    }

    // 타겟 관리
    public void SetSkillTarget(GameObject target)
    {
        if (activeSkillInstance != null)
        {
            activeSkillInstance.target = target;
        }
    }
    
    public void DetectEnemy()
    {
        if (SkillInfos.Count == 0 || skillInstances.Count == 0) return;

        Collider[] enemies = Physics.OverlapSphere(transform.position, SkillInfos[0].AttackDistance, resourceLayer);

        if (enemies.Length > 0)
        {
            GameObject nearestEnemy = GetNearestEnemy(enemies);

            if (nearestEnemy != null)
            {
                UseSkillOnResource(nearestEnemy);
                Fsm.ChangeState(FSM_CharacterState.FSM_CharacterState_Skill);
            }
        }
    }

    private GameObject GetNearestEnemy(Collider[] enemies)
    {
        GameObject nearestEnemy = null;
        float shortestDistance = Mathf.Infinity;

        foreach (Collider enemy in enemies)
        {
            float distance = Vector3.Distance(transform.position, enemy.transform.position);
            if (distance < shortestDistance)
            {
                shortestDistance = distance;
                nearestEnemy = enemy.gameObject;
            }
        }

        return nearestEnemy;
    }
    
    public void RotateToTarget(Transform target)
    {
        Vector3 direction = (target.position - transform.position).normalized;
        
        if (direction != Vector3.zero)
        {
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, 5f * Time.deltaTime);
        }
    }
    
    // 상태 체크 및 변경
    public void CheckSkillRangeAndExecute()
    {
        if (activeSkillInstance != null && activeSkillInstance.target != null)
        {
            if (IsWithinSkillRange(activeSkillInstance.target, activeSkillInstance.info.AttackDistance))
            {
                ExecuteActiveSkill();
            }
            else
            {
                Fsm.ChangeState(FSM_CharacterState.FSM_CharacterState_Move);
                SetDestination(activeSkillInstance.target.transform.position, true);
            }
        }
    }
    
    public void ResetSkillAndMove(Vector3 destination)
    {
        if (activeSkillInstance != null)
        {
            activeSkillInstance.target = null;
            activeSkillInstance = null;
            skillInstances.Clear();
        }
        Fsm.ChangeState(FSM_CharacterState.FSM_CharacterState_Move);
        SetDestination(destination);
    }

    public void CheckArrivalAndHandleState()
    {
        if (Vector3.Distance(transform.position, moveTarget) <= 0.1f ||
            (!_agent.pathPending && _agent.remainingDistance <= _agent.stoppingDistance && _agent.velocity.sqrMagnitude < 0.01f))
        {
            _agent.ResetPath();
            Fsm.ChangeState(FSM_CharacterState.FSM_CharacterState_Idle);
        }
    }
    
    public void SetMoveTarget(Vector3 target)
    {
        SetDestination(target);
    }
    
    private void HandleSkillButtonClicked(int skillIndex)
    {
        if (SelectionManager.Instance.unitSelected.Contains(this.gameObject))
        {
            if (skillIndex == 1 && _unitMana.currrentUnitMP >= 70f)
            {
                Debug.Log("click");
                UseSkillOnResource(null);
                Fsm.ChangeState(FSM_CharacterState.FSM_CharacterState_MagicSkill);
            }
            else if (skillIndex == 0)
            {
                UIManager.Instance.ShowBuildingUI(this);
            }
        }
    }
    
    public void CastMagicSkill(Vector3 targetPosition)
    {
        if (activeSkillInstance != null && activeSkillInstance.info.SkillPrefab != null)
        {
            _unitMana.UseMP(50f);
            Instantiate(activeSkillInstance.info.SkillPrefab, targetPosition, Quaternion.identity);
            Fsm.ChangeState(FSM_CharacterState.FSM_CharacterState_Idle);
        }
    }

    
    
    //데미지
    public void TakeDamage(float damage)
    {
        _unitHealth.HPTakeDamage(damage);

        if (_unitHealth.IsDeath())
        {
            Fsm.ChangeState(FSM_CharacterState.FSM_CharacterState_Dead);
        }
    }
}