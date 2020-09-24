using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectTower
{
    public class LeafWind : CardSpecial
    {
        [SerializeField] GameObject healVfx;

        private DaoshiAutoAttack daoshi;

        protected override void SetLifetime()
        {
            lifetime = 3f;
        }

        // Start is called before the first frame update
        void Start()
        {
            StartCoroutine(PushDot());
            daoshi = FindObjectOfType<DaoshiAutoAttack>();
        }

        private IEnumerator PushDot()
        {
            yield return new WaitForSeconds(0.5f);
            while (true)
            {
                foreach (Collider c in Physics.OverlapSphere(
                    transform.position, ((PointAoe)card).radius,
                    LayerMask.GetMask("Enemy")))
                {
                    var direction = Vector3.ProjectOnPlane(
                        c.gameObject.transform.position - transform.position,
                        new Vector3(0, 1, 0)).normalized;
                    var force = direction * ((PointAoe)card).force;
                    c.GetComponent<Rigidbody>().AddForce(force, ForceMode.Impulse);
                }
                yield return new WaitForSeconds(0.5f);
            }
        }

        private void HealOnDestroy()
        {
            GlobalAudioManager.Instance.Play("HealWood", transform.position);
            foreach (Collider c in Physics.OverlapSphere(
                transform.position, ((PointAoe)card).radius,
                LayerMask.GetMask("Player", "Minion")))
            {
                Instantiate(healVfx, c.gameObject.transform);
                c.GetComponent<Health>().HealAmount(daoshi.healPower * 10);
            }
        }

        protected override void SelfDestroy()
        {
            HealOnDestroy();
            base.SelfDestroy();
        }
    }
}