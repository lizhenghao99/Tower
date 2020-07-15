using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TowerUtils;

public class Health : MonoBehaviour
{
    [SerializeField] public int maxHealth = 100;
    public int currHealth { get; protected set; }

    public EventHandler healthChanged;

    public bool isDead = false;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        currHealth = maxHealth;
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        
    }

    public virtual void TakeDamage(int damage)
    {
        currHealth = Mathf.Clamp(currHealth - damage, 0, maxHealth);
        OnHealthChanged();
        if (currHealth <= 0)
        {
            Die();
        }
    }

    public virtual void TakeDamagePercent(float percent)
    {
        currHealth = Mathf.Clamp(
            currHealth - (int)(maxHealth * percent), 0, maxHealth);
        OnHealthChanged();
        if (currHealth <= 0)
        {
            Die();
        }
    }

    public virtual void HealAmount(int amount)
    {
        currHealth = Mathf.Clamp(currHealth + amount, 0, maxHealth);
        OnHealthChanged();
    }

    public virtual void HealPercent(float percent)
    {
        currHealth = Mathf.Clamp(
            currHealth + (int)(maxHealth * percent), 0, maxHealth);
        OnHealthChanged();
    }

    public virtual void Die()
    {
        currHealth = 0;

        GetComponentInChildren<Animator>().SetBool("Death", true);
        isDead = true;

        StartCoroutine(Utils.Timeout(() =>
            gameObject.SetActive(false)
            , 3f));
        
    }

    protected void OnHealthChanged()
    {
        healthChanged?.Invoke(gameObject, EventArgs.Empty);
    }
}
