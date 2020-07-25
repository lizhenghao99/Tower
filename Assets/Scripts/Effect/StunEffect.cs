using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class StunEffect : Effect
{
    private NavMeshAgent agent;
    private AttackBase attack;
    private Animator animator;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        attack = GetComponent<AttackBase>();
        animator = GetComponentInChildren<Animator>();
    }

    protected override void OnStart()
    {
        if (!GetComponent<Health>().immuneStun)
        {
            agent.isStopped = true;
            attack.enabled = false;
            if (animator != null)
            {
                animator.SetFloat("Velocity", 0f);
            }
            base.OnStart();
        }
        else
        {
            OnFinish();
        }
    }

    protected override void OnFinish()
    {
        agent.isStopped = false;
        attack.enabled = true;
        base.OnFinish();
    }
}
