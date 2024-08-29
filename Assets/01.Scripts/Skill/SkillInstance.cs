using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillInstance : MonoBehaviour
{
    public SkillInfo info;
    public GameObject target;
    //public float Cooltime;
    
    protected Character _character;
    private Dictionary<int, GameObject> _hittedTarget = new();

    private void Awake()
    {
        _character = GetComponent<Character>();
    }
    
    public virtual void EnterSkill()
    {
        _hittedTarget.Clear();
        
    }
    
    public virtual void ExecuteSkill()
    {
        
    }

    public virtual void ExitSkill()
    {
        
    }
    
    public void OnCollisionEnter(Collision other)
    {
        if (_hittedTarget.ContainsKey(other.gameObject.GetInstanceID()))
        {
            // 데미지 판정 
        }
        else
        {
            _hittedTarget.Add(other.gameObject.GetInstanceID(), other.gameObject);
        }
    }

    // public void StartCooltime()
    // {
    //     Cooltime = info.Cooltime;
    //     StartCoroutine(CooltimeCoroutine());
    // }
    //
    // private IEnumerator CooltimeCoroutine()
    // {
    //     while (Cooltime > 0)
    //     {
    //         Cooltime -= Time.deltaTime;
    //         yield return null;
    //     }
    //     Cooltime = 0;
    // }
}
