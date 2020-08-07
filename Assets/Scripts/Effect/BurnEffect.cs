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

    protected override void OnStart()
    {
        var tu = GetComponent<StunEffect>();
        if (tu != null)
        {
            tu.Enhance();
        }

        var shui = GetComponent<FreezeEffect>();
        if (shui != null)
        {
            shui.Kill();
            OnFinish();
            return;
        }

        base.OnStart();
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
