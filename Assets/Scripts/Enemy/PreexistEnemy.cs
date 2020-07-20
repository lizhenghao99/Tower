using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PreexistEnemy : Enemy
{
    private AttackBase attack;

    protected override void Awake()
    {
        // do nothing
    }

    protected override void Start()
    {
        attack = GetComponent<AttackBase>();
        attack.enabled = false;
    }

    public override void Spawn()
    {
        attack.enabled = true;
    }
}
