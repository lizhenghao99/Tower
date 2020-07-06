using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectManager : Singleton<EffectManager>
{
    [SerializeField] GameObject burnVfx;
    [SerializeField] GameObject freezeVfx;
    [SerializeField] GameObject stunVfx;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void Register(GameObject target, Effect.Type effectType, 
        float duration, float amount)
    {
        GameObject vfx = null;

        switch (effectType)
        {
            case Effect.Type.None:
                return;
            case Effect.Type.Burn:
                vfx = burnVfx;
                break;
            case Effect.Type.Freeze:
                vfx = freezeVfx;
                break;
            case Effect.Type.Stun:
                vfx = stunVfx;
                break;
            default:
                break;
        }

        Type T = Type.GetType(effectType + "Effect");

        Effect oldEffect = (Effect) target.GetComponent(T);
        if (oldEffect)
        {
            oldEffect.Extend(duration, amount);
        }
        else
        {
            Effect newEffect = (Effect) target.AddComponent(T);
            newEffect.Init(duration, amount, vfx);
        }
    }
}
