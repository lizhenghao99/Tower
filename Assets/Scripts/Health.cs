using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] ProgressBarPro healthbar;
    [SerializeField] int maxHealth = 100;
    private int currHealth;

    // Start is called before the first frame update
    void Start()
    {
        currHealth = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TakeDamage(int damage)
    {
        currHealth -= damage;
        healthbar.SetValue(currHealth, maxHealth);
        
        if (currHealth <= 0)
        {
            Destroy(transform.parent.gameObject);
        }
    }
}
