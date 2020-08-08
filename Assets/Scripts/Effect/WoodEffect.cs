using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WoodEffect : Effect
{
    private float timer;
    private AttackBase attack;
    private int originalAttackDamage;

    private void Awake()
    {
        attack = GetComponent<AttackBase>();
        originalAttackDamage = attack.attackDamage;
        type = Type.Wood;
        timer = 0;
    }

    protected override void OnStart()
    {
        var huo = GetComponent<BurnEffect>();
        if (huo != null)
        {
            huo.Enhance();
        }

        attack.attackDamage = (int) (attack.attackDamage * 0.8f);
        base.OnStart();
    }

    public override void Update()
    {
        timer -= Time.deltaTime;
        if (timer < 0)
        {
            timer = 1f;
            foreach (Collider c in 
                Physics.OverlapSphere(gameObject.transform.position, 3f))
            {
                if (c.gameObject.CompareTag("Minion"))
                {
                    c.gameObject.GetComponent<Health>().HealPercent(0.03f);
                }
                else if (c.gameObject.CompareTag("Player"))
                {
                    c.gameObject.GetComponent<Health>().HealPercent(0.01f);
                }
            }
        }
        base.Update();
    }

    protected override void OnFinish()
    {
        attack.attackDamage = originalAttackDamage;
        base.OnFinish();
    }
}
