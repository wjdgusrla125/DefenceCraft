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

    public Transform nexusTarget;
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
        nexusTarget = GameManager.Instance.Nexus.transform;
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
                RotateToTarget(nearestObstacle);  // 회전 추가

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
            _agent.SetDestination(nexusTarget.position);
            RotateToTarget(nexusTarget);  // 회전 추가
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
