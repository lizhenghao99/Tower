using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectTower
{
    public class DirtTrap : CardSpecial
    {
        private EffectManager effectManager;
        private DaoshiAutoAttack daoshi;

        protected override void SetLifetime()
        {
            // do nothing
        }

        private void Start()
        {
            StartCoroutine(Dot());
            effectManager = FindObjectOfType<EffectManager>();
            daoshi = FindObjectOfType<DaoshiAutoAttack>();
        }

        private IEnumerator Dot()
        {
            yield return new WaitForSeconds(1f);
            while (true)
            {
                foreach (Collider c in Physics.OverlapSphere(
                    transform.position, ((PointAoe)card).radius,
                    LayerMask.GetMask("Enemy")))
                {
                    c.GetComponent<Health>().TakeDamage(((PointAoe)card).damage);
                    effectManager.Register(daoshi.gameObject, c.gameObject,
                        Effect.Type.Stun,
                        UnityEngine.Random.Range(1f, 2f),
                        ((PointAoe)card).effectAmount);
                }
                yield return new WaitForSeconds(2f);
            }
        }
    }
}