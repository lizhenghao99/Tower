using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CardSpecial : MonoBehaviour
{
    [SerializeField] protected float lifetime;
    protected Card card;
    protected bool started = false;

    public void SetCard(Card c)
    {
        card = c;
        OnStart();
    }

    protected virtual void Update()
    {
        if (started)
        {
            lifetime -= Time.deltaTime;
            if (lifetime < 0)
            {
                SelfDestroy();
            }
        }  
    }

    protected virtual void OnStart()
    {
        SetLifetime();
        started = true;
    }

    protected abstract void SetLifetime();

    protected virtual void SelfDestroy()
    {
        Destroy(gameObject);
    }
}
