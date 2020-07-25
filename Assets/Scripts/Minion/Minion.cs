using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Minion : AttackBase
{
    [HideInInspector] public bool taunt;
    [HideInInspector] public bool invisible;
    [HideInInspector] public bool guard;
    [HideInInspector] public bool charge;
    [HideInInspector] public bool ambush;


    [HideInInspector] public Vector3 guardPosition;
    [HideInInspector] public float guardRadius;
    [HideInInspector] public Vector3 initialPosition;

    private bool inCombat = true;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        layerMask = LayerMask.GetMask("Enemy");
        LevelController.Instance.StageClear += OnStageClear;
    }

    // Update is called once per frame

    protected override void Update()
    {
        // anmiation
        animator.SetFloat("Velocity", agent.velocity.magnitude);
        if (agent.velocity.magnitude > Mathf.Epsilon)
        {
            spriteRenderer.flipX = agent.velocity.x < -0.3;
        }

        if (!inCombat || health.isDead)
        {
            agent.destination = transform.position;
            agent.isStopped = true;
            return;
        }


        if (charge)
        {
            attackRange = 100;
            GetEnemies(gameObject.transform.position, attackRange);
            SetTarget();
            Attack();
        }

        if (guard)
        {
            GetEnemies(guardPosition, guardRadius+0.5f);
            SetTarget();
            Attack();
            if (target == null)
            {
                agent.stoppingDistance = 0;
                agent.SetDestination(initialPosition);
            }
        }

        if (taunt)
        {
            ApplyTaunt();
        }
    }

    protected override void ApplyTaunt()
    {
        if (enemiesInRange == null) return;
        foreach(Collider c in enemiesInRange)
        {
            EffectManager.Instance.Taunt(gameObject, c.gameObject, 0);
        }
    }


    protected virtual void OnStageClear(object sender, EventArgs e)
    {
        inCombat = false;
    }
}
