using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LubanAutoAttack : PlayerAutoAttack
{
    private EffectManager effectManager;

    protected override void Start()
    {
        base.Start();
        effectManager = FindObjectOfType<EffectManager>();
    }

    protected override void SpecialAutoAttack()
    {
        GetComponent<LubanResource>().ResourceAutoGen();
    }

    protected override void ApplyTaunt()
    {
        if (enemiesInRange == null) return;
        foreach(Collider c in enemiesInRange)
        {
            effectManager.Taunt(gameObject, c.gameObject, 1);
        }
    }
}
