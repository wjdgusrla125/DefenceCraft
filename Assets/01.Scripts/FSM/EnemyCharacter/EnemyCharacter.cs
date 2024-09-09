/*
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyCharacter : CharacterBase<FSM_Enemy>
{
    [Header("EnemyCharacter")]
    [NonSerialized] public Animator _animator;

    private NavMeshAgent _agent;
    public EnemyHealth _enemyHealth;

    public GameObject nexusTarget;
    public float detectionRange = 10f;
    public float attackDistance = 2f;
    public LayerMask obstacleLayer;
    public float damage = 10f;
    public float rotationSpeed = 5f;

    public static readonly int EIdleHash = Animator.StringToHash("Idle");
    public static readonly int EMoveHash = Animator.StringToHash("Move");
    public static readonly int EAttackHash = Animator.StringToHash("Attack");
    public static readonly int EDeadHash = Animator.StringToHash("Dead");

    protected override void Awake()
    {
        base.Awake();

        _agent = GetComponent<NavMeshAgent>();
        _animator = GetComponent<Animator>();
        _enemyHealth = GetComponent<EnemyHealth>();
        
        _agent.updateRotation = false;
    }

    private void OnEnable()
    {
        nexusTarget = GameManager.Instance.Nexus;
    }

    public void DetectTarget()
    {
        Collider[] obstacles = Physics.OverlapSphere(transform.position, detectionRange, obstacleLayer);

        if (obstacles.Length > 0)
        {
            Transform nearestObstacle = GetNearestObstacle(obstacles);

            if (nearestObstacle != null)
            {
                _agent.SetDestination(nearestObstacle.position);
                RotateToTarget(nearestObstacle);

                if (Vector3.Distance(transform.position, nearestObstacle.position) <= attackDistance)
                {
                    Fsm.ChangeState(FSM_EnemyState.FSM_EnemyState_Attack);
                }
                else
                {
                    Fsm.ChangeState(FSM_EnemyState.FSM_EnemyState_Move);
                }
            }
        }
        else
        {
            _agent.SetDestination(nexusTarget.transform.position);
            RotateToTarget(nexusTarget.transform); 
            Fsm.ChangeState(FSM_EnemyState.FSM_EnemyState_Move);
        }
    }

    public Transform GetNearestObstacle(Collider[] obstacles)
    {
        Transform nearestObstacle = null;
        float shortestDistance = Mathf.Infinity;

        foreach (Collider obstacle in obstacles)
        {
            float distance = Vector3.Distance(transform.position, obstacle.transform.position);
            if (distance < shortestDistance)
            {
                shortestDistance = distance;
                nearestObstacle = obstacle.transform;
            }
        }

        return nearestObstacle;
    }

    public void TakeDamage(float damage)
    {
        _enemyHealth.HPTakeDamage(damage);

        if (_enemyHealth.IsDeath())
        {
            Fsm.ChangeState(FSM_EnemyState.FSM_EnemyState_Dead);
        }
    }

    public void RotateToTarget(Transform target)
    {
        Vector3 direction = (target.position - transform.position).normalized;
        
        if (direction != Vector3.zero)
        {
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, rotationSpeed * Time.deltaTime);
        }
    }
}
*/

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyCharacter : CharacterBase<FSM_Enemy>
{
    [Header("EnemyCharacter")]
    [NonSerialized] public Animator _animator;

    private NavMeshAgent _agent;
    public EnemyHealth _enemyHealth;

    public GameObject nexusTarget;
    public float detectionRange = 10f;
    public float attackDistance = 2f;
    public LayerMask obstacleLayer;
    public float damage = 10f;
    public float rotationSpeed = 5f;

    public static readonly int EIdleHash = Animator.StringToHash("Idle");
    public static readonly int EMoveHash = Animator.StringToHash("Move");
    public static readonly int EAttackHash = Animator.StringToHash("Attack");
    public static readonly int EDeadHash = Animator.StringToHash("Dead");

    protected override void Awake()
    {
        base.Awake();

        _agent = GetComponent<NavMeshAgent>();
        _animator = GetComponent<Animator>();
        _enemyHealth = GetComponent<EnemyHealth>();
        
        _agent.updateRotation = false;
    }

    private void Start()
    {
        nexusTarget = GameManager.Instance.Nexus;
        if (nexusTarget == null)
        {
            Debug.LogError("Nexus target is null in Start!");
        }
        else
        {
            DetectTarget();
        }
    }

    public void DetectTarget()
    {
        if (nexusTarget == null)
        {
            Debug.LogError("Nexus target is null in DetectTarget!");
            return;
        }

        Collider[] obstacles = Physics.OverlapSphere(transform.position, detectionRange, obstacleLayer);

        if (obstacles.Length > 0)
        {
            Transform nearestObstacle = GetNearestObstacle(obstacles);

            if (nearestObstacle != null)
            {
                SetDestinationAndRotate(nearestObstacle.position, nearestObstacle);

                if (Vector3.Distance(transform.position, nearestObstacle.position) <= attackDistance)
                {
                    Fsm.ChangeState(FSM_EnemyState.FSM_EnemyState_Attack);
                }
                else
                {
                    Fsm.ChangeState(FSM_EnemyState.FSM_EnemyState_Move);
                }
            }
        }
        else
        {
            SetDestinationAndRotate(nexusTarget.transform.position, nexusTarget.transform);
            Fsm.ChangeState(FSM_EnemyState.FSM_EnemyState_Move);
        }
    }

    private void SetDestinationAndRotate(Vector3 destination, Transform target)
    {
        if (_agent.isActiveAndEnabled)
        {
            _agent.SetDestination(destination);
            RotateToTarget(target);
        }
        else
        {
            Debug.LogWarning("NavMeshAgent is not active or enabled!");
        }
    }

    public Transform GetNearestObstacle(Collider[] obstacles)
    {
        Transform nearestObstacle = null;
        float shortestDistance = Mathf.Infinity;

        foreach (Collider obstacle in obstacles)
        {
            float distance = Vector3.Distance(transform.position, obstacle.transform.position);
            if (distance < shortestDistance)
            {
                shortestDistance = distance;
                nearestObstacle = obstacle.transform;
            }
        }

        return nearestObstacle;
    }

    public void TakeDamage(float damage)
    {
        _enemyHealth.HPTakeDamage(damage);

        if (_enemyHealth.IsDeath())
        {
            Fsm.ChangeState(FSM_EnemyState.FSM_EnemyState_Dead);
        }
    }

    public void RotateToTarget(Transform target)
    {
        Vector3 direction = (target.position - transform.position).normalized;
        
        if (direction != Vector3.zero)
        {
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, rotationSpeed * Time.deltaTime);
        }
    }
}