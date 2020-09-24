using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace ProjectTower
{
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
            if (attack.attackRate != 0)
            {
                var attackFrequency = 1 / attack.attackRate;
                attackFrequency *= 1 + amount;
                attack.attackRate = 1 / attackFrequency;
            }
            base.OnStart();
        }

        protected override void OnFinish()
        {
            attack.attackDamage = originalAttackDamage;
            attack.attackRate = originalAttackRate;
            base.OnFinish();
        }
    }
}