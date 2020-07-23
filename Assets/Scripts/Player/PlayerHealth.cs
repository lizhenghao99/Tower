using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System;

public class PlayerHealth : Health
{
    public int maxShield { get; private set; }
    public int currShield { get; private set; }

    public EventHandler shieldChanged;


    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        currShield = 0;
        maxShield = maxHealth;     
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
        GetComponentInChildren<Animator>().SetBool("Shield", currShield > 0);
    }

    public void AddShield(int amount)
    {
        currShield = Mathf.Clamp(currShield + amount, 0, maxShield);
        OnShieldChanged();
    }

    public void AddShieldPercent(float percent)
    {
        currShield = Mathf.Clamp(
            currShield + (int)(percent * maxShield), 0, maxShield);
        OnShieldChanged();
    }

    public override void TakeDamage(int damage)
    {
        ChangeHealth(-damage);
    }

    public override void TakeDamagePercent(float percent)
    {
        var damage = (int)percent * maxHealth;
        ChangeHealth(-damage);
    }

    private void ChangeHealth(int amount)
    {
        if (amount < 0)
        {
            if (currShield > 0)
            {
                currShield += (int) (amount * 0.7);
            }
            else
            {
                currShield += amount;
            }

            if (currShield < 0)
            {
                currHealth = Mathf.Clamp(currHealth + currShield, 0, maxHealth);
                currShield = 0;
            }
        }
        else
        {
            currHealth = Mathf.Clamp(currHealth + amount, 0, maxHealth);
        }

        OnShieldChanged();
        OnHealthChanged();

        if (currHealth <= 0)
        {
            Die();
        }
    }

    public override void Die()
    {
        GetComponentInChildren<Animator>().SetBool("Death", true);
        isDead = true;
        InstanceAudioManager audioManager = GetComponent<InstanceAudioManager>();
        if (audioManager != null)
        {
            audioManager.Play("Death");
        }
    }

    private void OnShieldChanged()
    {
        shieldChanged?.Invoke(gameObject, EventArgs.Empty);
    }
}
