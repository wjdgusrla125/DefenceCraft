using UnityEngine;

[CreateAssetMenu(menuName = "FSM/States/Dead")]
public class DeadState : UnitState
{
    public override void Enter(Unit unit)
    {
        unit.Agent.isStopped = true;
        unit.gameObject.SetActive(false); // 또는 애니메이션 후 제거 처리
        Debug.Log($"{unit.name} 사망 처리됨");
    }

    public override void Execute(Unit unit)
    {
        // 아무 동작 없음
    }

    public override void Exit(Unit unit)
    {
        // Dead 상태는 일반적으로 Exit 안 함
    }
}