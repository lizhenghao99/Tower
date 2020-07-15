using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAttack : AttackBase
{
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        layerMask = LayerMask.GetMask("Base");
    }

    // Update is called once per frame
    protected override void Update()
    {
        // anmiation
        animator.SetFloat("Velocity", agent.velocity.magnitude);
        if (agent.velocity.magnitude > Mathf.Epsilon)
        {
            spriteRenderer.flipX = agent.velocity.x > 0.1;
        }
        if (health.isDead)
        {
            agent.destination = gameObject.transform.position;
            agent.isStopped = true;
            return;
        }

        GetEnemies(gameObject.transform.position, 10000);
        SetTarget();
        Attack();
    }

    protected override void FlipX(RaycastHit hitInfo)
    {
        spriteRenderer.flipX =
               hitInfo.point.x > gameObject.transform.position.x;
    }
}

