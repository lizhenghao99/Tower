using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrostTrap : CardSpecial
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
                    Effect.Type.Freeze, 1f, ((PointAoe)card).effectAmount);
            }
            yield return new WaitForSeconds(1f);
        }
    }
}
