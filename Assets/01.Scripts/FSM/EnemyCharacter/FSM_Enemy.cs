using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum FSM_EnemyState
{
    None,
    FSM_EnemyState_Idle,
    FSM_EnemyState_Move,
    FSM_EnemyState_Attack,
    FSM_EnemyState_Dead
}

public class FSM_Enemy : StateMachine<FSM_EnemyState>
{
    
}
