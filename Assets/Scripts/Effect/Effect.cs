﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectTower
{
    public abstract class Effect : MonoBehaviour
    {
        public enum Type
        {
            None, Burn, Freeze, Stun, Rage, Wood, Lightning,
            FiveSwords, SevenSwords
        };

        public event EventHandler Start;
        public event EventHandler Finish;

        public Type type { get; protected set; }
        public GameObject caster;
        public float duration;
        public float amount;
        protected GameObject effectVfx;

        protected GameObject currentVfx;

        protected Vector3 originalPos;
        protected Vector3 originalScale;

        public void Init(GameObject c, float d, float a, GameObject vfx)
        {
            caster = c;
            duration = d;
            amount = a;
            effectVfx = vfx;

            OnStart();
        }

        public virtual void Extend(GameObject c, float d, float a)
        {
            caster = c;
            duration += d;
            amount = a;
        }

        public virtual void Enhance()
        {
            duration = Mathf.Clamp(duration * 1.5f, 0, 30f);
        }

        public virtual void Kill()
        {
            OnFinish();
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
            originalPos = currentVfx.transform.localPosition;
            originalScale = currentVfx.transform.localScale;
            var scale = GetComponentInChildren<SpriteRenderer>().bounds.size;
            currentVfx.transform.localScale =
                Vector3.one * (scale.magnitude * 0.3f);
            currentVfx.transform.position +=
                new Vector3(0f, 0.2f * scale.y, 0f);
            Start?.Invoke(this, EventArgs.Empty);
        }

        protected void InvokeStart(object sender)
        {
            Start?.Invoke(sender, EventArgs.Empty);
        }

        protected virtual void OnFinish()
        {
            Destroy(currentVfx);
            Destroy(this);
            Finish?.Invoke(this, EventArgs.Empty);
        }

        protected void InvokeFinish(object sender)
        {
            Finish?.Invoke(sender, EventArgs.Empty);
        }
    }
}