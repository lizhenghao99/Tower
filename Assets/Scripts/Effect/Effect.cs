using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Effect : MonoBehaviour
{
    public enum Type { None, Burn, Freeze, Stun };

    public EventHandler start;
    public EventHandler finish;

    public Type type { get; protected set; }
    public GameObject caster;
    protected float duration;
    protected float amount;
    protected GameObject effectVfx;

    private GameObject currentVfx;

    public void Init(GameObject c, float d, float a, GameObject vfx)
    {
        caster = c;
        duration = d;
        amount = a;
        effectVfx = vfx;

        OnStart();
    }

    public void Extend(float d, float a)
    {
        duration = d;
        amount = a;
    }

    public virtual void Update()
    {
        duration -= Time.deltaTime;
        if (duration < 0)
        {
            OnFinish();
        }
    }

    protected virtual void OnStart()
    {
        currentVfx = Instantiate(effectVfx, gameObject.transform);
        var scale = GetComponent<CapsuleCollider>().radius;
        currentVfx.transform.localScale = new Vector3(1.2f*scale, 1.2f*scale, 1.2f*scale);
        start?.Invoke(this, EventArgs.Empty);
    }

    protected virtual void OnFinish()
    {
        Destroy(currentVfx);
        Destroy(this);
        finish?.Invoke(this, EventArgs.Empty);
    }
}
