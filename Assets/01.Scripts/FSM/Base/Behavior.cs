using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterBase<T> : BehaviorBase
{
    [Header("CharacterBase")]
    [NonSerialized] public T Fsm;
    [NonSerialized] public Rigidbody _rigidbody;
    
    protected virtual void Awake()
    {
        Fsm = GetComponent<T>();
        _rigidbody = GetComponent<Rigidbody>();
    }
}
