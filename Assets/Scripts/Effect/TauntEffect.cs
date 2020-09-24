using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace ProjectTower
{
    public class TauntEffect : Effect
    {
        private AttackBase attack;
        private bool isActive;

        private void Awake()
        {
            attack = GetComponent<AttackBase>();
        }

        public override void Update()
        {
            if (isActive)
            {
                base.Update();
                if (!caster.activeInHierarchy
                    || caster.GetComponent<Health>().isDead)
                {
                    OnFinish();
                }
            }
        }

        public override void Extend(GameObject c, float d, float a)
        {
            if (a < amount || !isActive || caster == c)
            {
                caster = c;
                duration = d;
                amount = a;
                OnStart();
            }
        }

        protected override void OnStart()
        {
            isActive = true;
            attack.taunter = caster;
            InvokeStart(this);
        }

        protected override void OnFinish()
        {
            isActive = false;
            attack.taunter = null;
            InvokeFinish(this);
        }
    }
}