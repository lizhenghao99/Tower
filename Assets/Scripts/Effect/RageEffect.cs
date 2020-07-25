using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class RageEffect : Effect
{
    private AttackBase attack;
    private int originalAttackDamage;
    private float originalAttackRate;

    private void Awake()
    {
        attack = GetComponent<AttackBase>();
    }

    protected override void OnStart()
    {
        originalAttackDamage = attack.attackDamage;
        originalAttackRate = attack.attackRate;

        attack.attackDamage = (int)(attack.attackDamage * (1 + amount));
        attack.attackRate *= 1 + amount;

        base.OnStart();
    }

    protected override void OnFinish()
    {
        attack.attackDamage = originalAttackDamage;
        attack.attackRate = originalAttackRate;
        base.OnFinish();
    }
}
