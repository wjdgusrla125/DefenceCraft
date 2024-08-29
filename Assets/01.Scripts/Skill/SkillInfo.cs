using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SkillInfo", menuName = "Skill/CreateSkillInfo")]
public class SkillInfo : ScriptableObject
{
    public int SkillIndex;
    public float AttackDistance;
    public string AnimationName;
    public float hitInterval;
    public int Damage;
    public GameObject SkillPrefab;
    
    [NonSerialized] public int AnimationName_Hash;

    private void OnEnable()
    {
        AnimationName_Hash = Animator.StringToHash(AnimationName);
    }
}
