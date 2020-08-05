using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DaoshiAutoAttack : RangedPlayerAutoAttack
{
    private EffectManager effectManager;

    protected override void Start()
    {
        base.Start();
        effectManager = FindObjectOfType<EffectManager>();
    }

    protected override void SpecialRangedAttack(Collider[] hits)
    {
        foreach (Collider c in hits)
        {
            effectManager.Register(gameObject, c.gameObject,
                Effect.Type.Freeze, 2, 0.5f);
        }
    }

    protected override void ApplyTaunt()
    {
        if (enemiesInRange == null) return;
        foreach(Collider c in enemiesInRange)
        {
            effectManager.Taunt(gameObject, c.gameObject, 2);
        }
    }
}
