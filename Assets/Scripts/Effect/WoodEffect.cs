using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectTower
{
    public class WoodEffect : Effect
    {
        public float healPlayerPercent;

        private float timer;
        private AttackBase attack;
        private int originalAttackDamage;

        private void Awake()
        {
            attack = GetComponent<AttackBase>();
            originalAttackDamage = attack.attackDamage;
            type = Type.Wood;
            timer = 0;
            healPlayerPercent = 0.02f;
        }

        protected override void OnStart()
        {
            var huo = GetComponent<BurnEffect>();
            if (huo != null)
            {
                huo.Enhance();
            }

            attack.attackDamage = (int)(attack.attackDamage * 0.75f);
            base.OnStart();
        }

        public override void Update()
        {
            timer -= Time.deltaTime;
            if (timer < 0)
            {
                timer = 1f;
                foreach (Collider c in
                    Physics.OverlapSphere(gameObject.transform.position,
                    GetComponent<Collider>().bounds.extents.x + 3f))
                {
                    if (c.gameObject.CompareTag("Minion"))
                    {
                        c.gameObject.GetComponent<Health>().HealAmount((int)amount);
                    }
                    else if (c.gameObject.CompareTag("Player"))
                    {
                        c.gameObject.GetComponent<Health>().HealPercent(healPlayerPercent);
                    }
                }
            }
            base.Update();
        }

        protected override void OnFinish()
        {
            attack.attackDamage = originalAttackDamage;
            base.OnFinish();
        }
    }
}