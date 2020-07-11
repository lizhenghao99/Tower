using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LubanAutoAttack : PlayerAutoAttack
{
    protected override void SpecialAutoAttack()
    {
        GetComponent<LubanResource>().ResourceAutoGen();
    }

    protected override void ApplyTaunt()
    {
        if (enemiesInRange == null) return;
        foreach(Collider c in enemiesInRange)
        {
            EffectManager.Instance.Taunt(gameObject, c.gameObject, 1);
        }
    }
}
