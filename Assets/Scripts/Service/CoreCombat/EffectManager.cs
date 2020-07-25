using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectManager : Singleton<EffectManager>
{
    [SerializeField] GameObject burnVfx;
    [SerializeField] GameObject freezeVfx;
    [SerializeField] GameObject stunVfx;
    [SerializeField] GameObject rageVfx;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void Register(GameObject caster, GameObject target, Effect.Type effectType, 
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
            case Effect.Type.Rage:
                vfx = rageVfx;
                break;
            default:
                break;
        }

        Type T = Type.GetType(effectType + "Effect");

        Effect oldEffect = (Effect) target.GetComponent(T);
        if (oldEffect)
        {
            oldEffect.Extend(caster, duration, amount);
        }
        else
        {
            Effect newEffect = (Effect) target.AddComponent(T);
            newEffect.Init(caster, duration, amount, vfx);
        }
    }

    public void Taunt(GameObject caster, GameObject target, float level)
    {
        float tauntDuration = 5;

        TauntEffect oldEffect = target.GetComponent<TauntEffect>();
        if (oldEffect)
        {
            oldEffect.Extend(caster, tauntDuration, level);
        }
        else
        {
            TauntEffect newEffect = target.AddComponent<TauntEffect>();
            newEffect.Init(caster, tauntDuration, level, null);
        }
    }
}
