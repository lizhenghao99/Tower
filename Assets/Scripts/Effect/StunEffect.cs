using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class StunEffect : Effect
{
    private NavMeshAgent agent;
    private EnemyAttack attack;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        attack = GetComponent<EnemyAttack>();
        
    }

    protected override void OnStart()
    {
        agent.isStopped = true;
        attack.enabled = false;
        base.OnStart();
    }

    protected override void OnFinish()
    {
        agent.isStopped = false;
        attack.enabled = true;
        base.OnFinish();
    }
}
