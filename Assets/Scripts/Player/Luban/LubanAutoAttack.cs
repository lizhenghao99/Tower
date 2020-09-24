using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ProjectTower;

namespace ProjectTower
{
    public class LubanAutoAttack : PlayerAutoAttack
    {
        [Header("Vfx")]
        [SerializeField] GameObject meleeVfx;
        [SerializeField] float vfxDelay;
        private EffectManager effectManager;

        protected override void Start()
        {
            base.Start();
            effectManager = FindObjectOfType<EffectManager>();
        }

        protected override void SpecialAutoAttack()
        {
            GetComponent<LubanResource>().ResourceAutoGen();
            StartCoroutine(Utils.Timeout(() =>
            {
                var fx = Instantiate(meleeVfx, gameObject.transform);
                if (targetHitInfo.point.x < gameObject.transform.position.x)
                {
                    fx.transform.rotation = Quaternion.Euler(0, 180, 0);
                }
            }, vfxDelay));
        }

        protected override void ApplyTaunt()
        {
            if (enemiesInRange == null) return;
            foreach (Collider c in enemiesInRange)
            {
                effectManager.Taunt(gameObject, c.gameObject, 1);
            }
        }
    }
}