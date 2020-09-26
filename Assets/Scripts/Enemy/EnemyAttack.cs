﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

namespace ProjectTower
{
    public class EnemyAttack : AttackBase
    {
        // Start is called before the first frame update
        protected override void Start()
        {
            base.Start();
            layerMask = LayerMask.GetMask("BaseTower");
        }

        // Update is called once per frame
        protected virtual void Update()
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

            GetEnemies(gameObject.transform.position, 100);
            SetTarget();
            if(TestTarget())
            {
                Chase();
                if (TestRange())
                {
                    Attack();
                }
            }     
        }

        protected override void FlipX(RaycastHit hitInfo)
        {
            spriteRenderer.flipX =
                   hitInfo.point.x > gameObject.transform.position.x;
        }
    }
}
