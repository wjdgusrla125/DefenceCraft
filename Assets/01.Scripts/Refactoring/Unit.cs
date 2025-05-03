using UnityEngine;
using UnityEngine.AI;

public class Unit : MonoBehaviour
{
    public NavMeshAgent Agent { get; private set; }
    public StateMachine StateMachine { get; private set; }
    public Transform Target { get; set; }
    
    private void Awake()
    {
        Agent = GetComponent<NavMeshAgent>();
        StateMachine = GetComponent<StateMachine>();
        StateMachine.unit = this;
    }
}