using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DaoshiAutoAttack : RangedPlayerAutoAttack
{
    private EffectManager effectManager;
    private DaoshiResource resource;

    protected override void Start()
    {
        base.Start();
        effectManager = FindObjectOfType<EffectManager>();
        resource = GetComponent<DaoshiResource>();
        StartCoroutine(resource.ResourceAutoGen());
    }

    protected override void ToSummonMissiles(int count, Quaternion rotation)
    {
        summonMissiles = StartCoroutine(SummonMissiles(count, rotation,
            resource.secondaryResource));
    }

    protected override void SpecialRangedAttack(Collider[] hits, int type)
    {
        switch (type)
        {
            case 2:
                foreach (Collider c in hits)
                {
                    effectManager.Register(gameObject, c.gameObject,
                        Effect.Type.Wood, 1f, attackDamage / 3f);
                }
                break;
            case 3:
                foreach (Collider c in hits)
                {
                    effectManager.Register(gameObject, c.gameObject,
                        Effect.Type.Freeze, 0.5f, 0.2f);
                }
                break;
            case 4:
                foreach (Collider c in hits)
                {
                    effectManager.Register(gameObject, c.gameObject,
                        Effect.Type.Burn, 1f, attackDamage / 3f);
                }
                break;
            case 5:
                var chance = UnityEngine.Random.Range(0f, 1f);
                if (chance < 0.3f)
                {
                    foreach (Collider c in hits)
                    {
                        effectManager.Register(gameObject, c.gameObject,
                            Effect.Type.Stun, 1f, 0);
                    }
                }
                break;
            default:
                return;
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
