using UnityEngine;
using UnityEngine.AI;

public enum FSM_CharacterState
{
    None,
    FSM_CharacterState_Idle,
    FSM_CharacterState_Move,
    FSM_CharacterState_Dead,
    FSM_CharacterState_Skill,
    FSM_CharacterState_MagicSkill
}

public class FSM_Character : StateMachine<FSM_CharacterState>
{
    
}