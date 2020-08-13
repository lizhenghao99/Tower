using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseTowerHealth : Health
{
    private InstanceAudioManager audioManager;
    private float hurtAudioTime;
    [Header("Vfx")] 
    [SerializeField] GameObject deathVfx;
    protected override void Start()
    {
        base.Start();
        audioManager = GetComponent<InstanceAudioManager>();
        hurtAudioTime = Time.time;
    }

    public override void TakeDamage(int damage)
    {
        GetComponentInChildren<Animator>().SetTrigger("Hurt");
        if (Time.time - hurtAudioTime > 1f)
        {
            if (audioManager != null)
            {
                audioManager.Play("Hurt");
            }
            hurtAudioTime = Time.time;
        }
        base.TakeDamage(damage);
    }

    public override void TakeDamagePercent(float percent)
    {
        GetComponentInChildren<Animator>().SetTrigger("Hurt");
        if (Time.time - hurtAudioTime > 1f)
        {
            if (audioManager != null)
            {
                audioManager.Play("Hurt");
            }
            hurtAudioTime = Time.time;
        }
        base.TakeDamagePercent(percent);
    }
    public override void Die()
    {
        Instantiate(deathVfx, gameObject.transform);
        GetComponentInChildren<Animator>().SetBool("Death", true);
        isDead = true;
        if (audioManager != null)
        {
            audioManager.Play("Death");
            audioManager.Play("Fall");
        }
        currHealth = 0;
        OnDeath();
    }
}
