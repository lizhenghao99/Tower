using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectTower
{
    public class SwordPierceProjectile : AngleProjectile
    {
        private void Start()
        {
            GetComponent<InstanceAudioManager>().Play("Launch");
        }

        protected override void Impact()
        {
            hasHit = true;
            var fx = Instantiate(impactEffect);
            fx.transform.position = gameObject.transform.position;
            base.Impact();
        }

        protected override void HitBehavior(Collider other)
        {
            if (other.CompareTag("Prop"))
            {
                SelfDestroy();
            }
            else
            {
                Impact();
            }
        }

        protected override void SelfDestroy()
        {
            var fx = Instantiate(impactEffect);
            fx.transform.position = gameObject.transform.position;
            Destroy(gameObject);
        }
    }
}