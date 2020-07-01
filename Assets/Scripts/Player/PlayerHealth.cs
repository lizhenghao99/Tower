using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] public int maxHealth = 1000;

    public int maxShield { get; private set; }
    public int currHealth { get; private set; }
    public int currShield { get; private set; }

    public EventHandler healthChanged;
    public EventHandler shieldChanged;

    

    // Start is called before the first frame update
    void Start()
    {
        currHealth = maxHealth;
        currShield = 0;
        maxShield = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddShield(int amount)
    {
        currShield = Mathf.Clamp(currShield + amount, 0, maxShield);
        OnShieldChanged();
    }

    public void ChangeHealth(int amount)
    {
        if (amount < 0)
        {
            currShield += amount;
            if (currShield < 0)
            {
                currHealth += currShield;
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
            currHealth = 0;
            gameObject.transform.parent.gameObject.SetActive(false);
        }
    }

    private void OnHealthChanged()
    {
        healthChanged?.Invoke(gameObject, EventArgs.Empty);
    }

    private void OnShieldChanged()
    {
        shieldChanged?.Invoke(gameObject, EventArgs.Empty);
    }
}
