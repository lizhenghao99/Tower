using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightningballProjectile : AngleProjectile
{
    private InstanceAudioManager audioManager;

    public override void SetCard(Angle c)
    {
        base.SetCard(c);
        audioManager = GetComponent<InstanceAudioManager>();
        StartCoroutine(Dot());
    }

    protected override void HitBehavior(Collider other)
    {
        // do nothing
    }

    private IEnumerator Dot()
    {
        yield return new WaitForSeconds(1f);
        while (true)
        {
            foreach (Collider c in Physics.OverlapSphere(
                gameObject.transform.position, card.radius))
            {
                if (c.CompareTag("Prop"))
                {
                    SelfDestroy();
                }
                else if (c.CompareTag("Enemy"))
                {
                    Impact();
                }
            }
            audioManager.Play("SpecialSpawn");
            yield return new WaitForSeconds(1f);
        }
    }
}
