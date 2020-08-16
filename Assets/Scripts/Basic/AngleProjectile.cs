using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class AngleProjectile : MonoBehaviour
{
    [SerializeField] GameObject impactEffect;
    protected Angle card;
    protected Vector3 startingPosition;
    protected EffectManager effectManager;
    protected PlayerController player;
    protected bool cardSet = false;

    protected bool hasHit = false;

    // Update is called once per frame
    protected virtual void Update()
    {
        if (cardSet)
        {
            var currDistance =
            Vector3.Distance(gameObject.transform.position, startingPosition);
            if (currDistance > card.range)
            {
                SelfDestroy();
            }
        }
    }

    protected virtual void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy") ||
            other.CompareTag("Prop"))
        {
            effectManager.Register(player.gameObject, other.gameObject,
                card.effect, card.effectDuration, card.effectAmount);
            HitBehavior();
        }
    }

    protected virtual void HitBehavior()
    {
        Impact();
        SelfDestroy();
    }

    protected virtual void Impact()
    {
        if (impactEffect != null && !hasHit)
        {
            hasHit = true;
            var fx = Instantiate(impactEffect);
            fx.transform.position = gameObject.transform.position;
        }
        foreach (Collider c in Physics.OverlapSphere(transform.position, card.radius, 
            LayerMask.GetMask("Enemy")))
        {
            effectManager.Register(player.gameObject, c.gameObject,
                card.effect, card.effectDuration, card.effectAmount);

            var direction = Vector3.ProjectOnPlane(
                c.gameObject.transform.position
                - transform.position, new Vector3(0, 1, 0)).normalized;
            var force = direction * card.force;

            c.GetComponent<Rigidbody>().AddForce(force, ForceMode.Impulse);
            c.GetComponent<Health>().TakeDamage(card.damage);
        }
    }

    public virtual void SetCard(Angle c)
    {
        cardSet = true;
        card = c;
        startingPosition = gameObject.transform.position;
        effectManager = FindObjectOfType<EffectManager>();
        player = FindObjectsOfType<PlayerController>()
            .Where(p => p.gameObject.name == card.owner.ToString())
            .FirstOrDefault();
    }

    protected virtual void SelfDestroy()
    {
        if (impactEffect != null && !hasHit)
        {
            var fx = Instantiate(impactEffect);
            fx.transform.position = gameObject.transform.position;
        }
        Destroy(gameObject);
    } 
}
