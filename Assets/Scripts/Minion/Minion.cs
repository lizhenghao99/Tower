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

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        layerMask = LayerMask.GetMask("Enemy");
    }

    // Update is called once per frame

    protected override void Update()
    {
        if (charge)
        {
            attackRange = 10000;
            GetEnemies(gameObject.transform.position, attackRange);
            SetTarget();
            Attack();
        }

        if (guard)
        {
            GetEnemies(guardPosition, guardRadius);
            SetTarget();
            Attack();
            if (target == null)
            {
                agent.stoppingDistance = 0;
                agent.SetDestination(initialPosition);
            }
        }
    }
}
