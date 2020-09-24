using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectTower
{
    public class SwordRain : CardSpecial
    {
        private int damage;
        private float radius;

        protected override void SetLifetime()
        {
            lifetime = 2f;
        }

        protected override void OnStart()
        {
            base.OnStart();
            damage = ((PointAoe)card).damage;
            radius = ((PointAoe)card).radius;
            StartCoroutine(AoeDot());
        }

        protected IEnumerator AoeDot()
        {
            while (true)
            {
                foreach (Collider c in Physics.OverlapSphere(transform.position, radius,
                LayerMask.GetMask("Enemy")))
                {
                    c.GetComponent<Health>().TakeDamage(damage);
                }
                yield return new WaitForSeconds(0.25f);
            }
        }

        protected override void SelfDestroy()
        {
            base.SelfDestroy();
        }
    }
}