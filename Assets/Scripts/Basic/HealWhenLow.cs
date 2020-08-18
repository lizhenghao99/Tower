using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealWhenLow : MonoBehaviour
{
    private Health health;
    // Start is called before the first frame update
    void Start()
    {
        health = GetComponent<Health>();    
    }

    // Update is called once per frame
    void Update()
    {
        if (health.currHealth < health.maxHealth * 0.3)
        {
            health.HealPercent(1f);
        }
    }
}
