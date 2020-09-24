using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectTower
{
    public class EffectManager : MonoBehaviour
    {
        [SerializeField] GameObject burnVfx;
        [SerializeField] GameObject freezeVfx;
        [SerializeField] GameObject stunVfx;
        [SerializeField] GameObject rageVfx;
        [SerializeField] GameObject woodVfx;
        [SerializeField] GameObject lightningVfx;
        [SerializeField] GameObject fiveSwordsVfx;
        [SerializeField] GameObject sevenSwordsVfx;

        public event EventHandler<Effect> registerEvent;

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
                case Effect.Type.Wood:
                    vfx = woodVfx;
                    break;
                case Effect.Type.Lightning:
                    vfx = lightningVfx;
                    break;
                case Effect.Type.FiveSwords:
                    vfx = fiveSwordsVfx;
                    break;
                case Effect.Type.SevenSwords:
                    vfx = sevenSwordsVfx;
                    break;
                default:
                    break;
            }

            Type T = Type.GetType("ProjectTower." + effectType + "Effect");

            Effect oldEffect = (Effect)target.GetComponent(T);
            if (oldEffect)
            {
                oldEffect.Extend(caster, duration, amount);
            }
            else
            {
                Effect newEffect = (Effect)target.AddComponent(T);
                OnRegister(caster, newEffect);
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

        private void OnRegister(GameObject c, Effect e)
        {
            registerEvent?.Invoke(c, e);
        }
    }
}