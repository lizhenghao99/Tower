using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BurnEffect : Effect
{
    private float timer;

    private void Awake()
    {
        type = Type.Burn;
        timer = 1;
    }

    public override void Update()
    {
        timer -= Time.deltaTime;
        if (timer < 0)
        {
            timer = 1;
            GetComponent<Health>()?.TakeDamage((int)amount);
        }
        base.Update();
    }
}
