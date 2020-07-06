using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class FreezeEffect : Effect
{
    private NavMeshAgent agent;
    private float originalSpeed;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    protected override void OnStart()
    {
        originalSpeed = agent.speed;
        agent.speed *= (1 - amount);
        base.OnStart();
    }

    protected override void OnFinish()
    {
        agent.speed = originalSpeed;
        base.OnFinish();
    }
}
