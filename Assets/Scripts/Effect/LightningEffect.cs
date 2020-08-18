using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightningEffect : Effect
{
    private float timer;
    private EffectManager effectManager;

    private void Awake()
    {
        type = Type.Lightning;
        timer = 1;
        effectManager = FindObjectOfType<EffectManager>();
    }

    protected override void OnStart()
    {
        var shui = GetComponent<FreezeEffect>();
        if (shui != null)
        {
            shui.Enhance();
        }

        base.OnStart();
    }

    public override void Update()
    {
        timer -= Time.deltaTime;
        if (timer < 0)
        {
            timer = 1;
            GetComponent<Health>()?.TakeDamagePercent(0.02f);
            if (amount > 0)
            {
                Spread();
            }
        }
        base.Update();
    }

    private void Spread()
    {
        var count = 0;
        foreach (Collider c in Physics.OverlapSphere(transform.position,
            GetComponent<Collider>().bounds.extents.x + 5f))
        {
            if (c.CompareTag("Enemy"))
            {
                var prevLightning = c.GetComponent<LightningEffect>();
                if (prevLightning == null)
                {
                    effectManager.Register(gameObject, c.gameObject,
                        Type.Lightning, duration, amount - 1);
                    count++;
                }
            }
            if (count >= amount)
            {
                break;
            }
        }
    }
}
