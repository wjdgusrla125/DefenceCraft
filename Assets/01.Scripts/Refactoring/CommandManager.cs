using UnityEngine;
using UnityEngine.EventSystems;

public class CommandManager : SceneSingleton<CommandManager>
{
    public enum CommandType { None, Move, Attack }
    
    [SerializeField] private CommandType currentCommand = CommandType.None;
    
    private void Update()
    {
        if (EventSystem.current.IsPointerOverGameObject()) return;

        if (Input.GetMouseButtonDown(1))
        {
            HandleRightClick(); // 우클릭
        }
        else if (Input.GetMouseButtonDown(0))
        {
            if (currentCommand == CommandType.Move || currentCommand == CommandType.Attack)
            {
                HandleRightClick();
                
            }
        }
    }
    
    private void HandleRightClick()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            GameObject target = hit.collider.gameObject;

            if (target.layer == LayerMask.NameToLayer("Enemy"))
            {
                // 공격 명령
                foreach (var unit in SelectionManager.Instance.GetSelectedUnits())
                {
                    if (unit.TryGetComponent<Unit>(out var realUnit))
                    {
                        realUnit.Target = hit.collider.transform;
                        IssueAttackCommand(realUnit);
                    }
                }
            }
            else if (target.layer == LayerMask.NameToLayer("Ground"))
            {
                // 단순 이동 명령
                IssueMoveCommand(hit.point);
            }
        }
        
        currentCommand = CommandType.None;
    }
    
    public void SetCommand(CommandType command)
    {
        currentCommand = command;
    }
    
    private void IssueMoveCommand(Vector3 destination)
    {
        foreach (var unit in SelectionManager.Instance.GetSelectedUnits())
        {
            if (unit.TryGetComponent<Unit>(out var realUnit))
            {
                realUnit.Target = null;
                realUnit.Agent.SetDestination(destination);

                UnitState moveState = FindMoveState(realUnit.StateMachine);

                if (moveState != null && realUnit.StateMachine.CanChangeTo(moveState))
                {
                    realUnit.StateMachine.ChangeState(moveState);
                }
            }
        }
    }
    
    private void IssueAttackCommand(Unit unit)
    {
        UnitState attackState = FindAttackState(unit.StateMachine, unit);
        if (attackState != null)
        {
            float distance = Vector3.Distance(unit.transform.position, unit.Target.position);
            float attackRange = GetAttackRange(attackState);

            if (distance <= attackRange)
            {
                unit.StateMachine.ChangeState(attackState);
            }
            else
            {
                unit.Agent.SetDestination(unit.Target.position);
                UnitState moveState = FindMoveState(unit.StateMachine);
                
                if (moveState != null)
                    unit.StateMachine.ChangeState(moveState);
            }
        }
    }
    
    private UnitState FindMoveState(StateMachine stateMachine)
    {
        foreach (var state in stateMachine.availableStates)
        {
            if (state is MoveState)
                return state;
        }
        return null;
    }
    
    private UnitState FindAttackState(StateMachine sm, Unit unit)
    {
        foreach (var state in sm.availableStates)
        {
            if (state is MeleeAttackState || state is RangedAttackState)
                return state;
        }
        return null;
    }

    private float GetAttackRange(UnitState state)
    {
        if (state is MeleeAttackState melee) return melee.attackRange;
        if (state is RangedAttackState ranged) return ranged.attackRange;
        return 0f;
    }
}
