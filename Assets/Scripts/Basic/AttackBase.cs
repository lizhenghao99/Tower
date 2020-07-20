using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;
using TowerUtils;

public abstract class AttackBase : MonoBehaviour
{
    [SerializeField] public int attackDamage = 20;
    [SerializeField] public float attackRange;
    [SerializeField] public float attackRate;
    [SerializeField] public float stopRange;
    [SerializeField] protected NavMeshAgent agent;
    [SerializeField] protected Animator animator;
    [SerializeField] protected SpriteRenderer spriteRenderer;

    [HideInInspector] public GameObject taunter;

    protected Collider[] enemiesInRange;
    protected GameObject target;
    protected int layerMask;
    protected float attackTimer;

    protected Health health;

    protected float meleeRange = 5f;

    protected bool isSpecialing = false;
    protected AttackEvent attackEvent;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        agent.updateRotation = false;
        agent.updateUpAxis = false;
        health = GetComponent<Health>();
        attackEvent = GetComponentInChildren<AttackEvent>();
        attackEvent.attack += OnAttack;
    }

    // Update is called once per frame
    protected abstract void Update();

    protected virtual void GetEnemies(Vector3 source, float range)
    {
        enemiesInRange = Physics.OverlapSphere(
            source, range, layerMask);
    }

    protected virtual void SetTarget()
    {
        target = null;

        if (TauntedBehavior()) return;

        if (enemiesInRange.Length == 0)
        {
            target = null;
            return;
        }

        Collider closestEnemy = null;
        float min_dist = Mathf.Infinity;
        foreach (Collider e in enemiesInRange)
        {
            if (e.gameObject.GetComponent<Health>().isDead) continue;
            float dist = Vector3.Distance(gameObject.transform.position,
                                            e.transform.position);
            if (dist < min_dist)
            {
                closestEnemy = e;
                min_dist = dist;
            }
        }
        if (closestEnemy)
        {
            target = closestEnemy.gameObject;
        }
    }

    protected virtual bool TauntedBehavior()
    {
        if (taunter && taunter.activeInHierarchy 
            && !taunter.GetComponent<Health>().isDead)
        {
            target = taunter;
            return true;
        }
        else
        {
            target = null;
            return false;
        }
    }

    protected virtual void ApplyTaunt()
    {
        // do nothing
    }

    protected virtual void Attack()
    {
        if (target == null || !target.activeInHierarchy) return;

        RaycastHit[] hitInfoArray;
        RaycastHit hitInfo;

        hitInfoArray = Physics.RaycastAll(gameObject.transform.position,
            target.transform.position - gameObject.transform.position);

        if (hitInfoArray.Length == 0) return;

        hitInfo = hitInfoArray.Where((h) => h.collider.gameObject == target)
            .FirstOrDefault();
        agent.stoppingDistance = stopRange;
        agent.SetDestination(hitInfo.point);

        if ((agent.velocity.magnitude < Mathf.Epsilon || stopRange < meleeRange)
            && Vector3.Distance(
                Vector3.ProjectOnPlane(agent.transform.position, new Vector3(0, 1, 0)),
                Vector3.ProjectOnPlane(target.transform.position, new Vector3(0, 1, 0)))
                < stopRange + 2)
        {
            attackTimer -= Time.deltaTime;
            FlipX(hitInfo);
            if (!isSpecialing)
            {
                if (attackTimer < 0)
                {
                    agent.isStopped = true;
                    animator.SetTrigger("Attack");
                    // attack behavior moved to animation
                    attackTimer = attackRate;
                }
            }
            SpeicalAttackUpdate();
        }
        else
        {
            attackTimer = attackRate;
        }
    }

    protected virtual void FlipX(RaycastHit hitInfo)
    {
        spriteRenderer.flipX =
               hitInfo.point.x < gameObject.transform.position.x;
    }

    protected virtual void SpecialAutoAttack()
    {
        // do nothing
    }

    protected virtual void SpeicalAttackUpdate()
    {
        // do nothing
    }

    protected virtual void OnAttack(object sender, EventArgs e)
    {
        ResumeFromAttack();

        if (target == null) return;

        if (Vector3.Distance(
                Vector3.ProjectOnPlane(agent.transform.position, new Vector3(0, 1, 0)),
                Vector3.ProjectOnPlane(target.transform.position, new Vector3(0, 1, 0)))
                < stopRange + 2)
        {
            SpecialAutoAttack();
            target.GetComponent<Health>()?.TakeDamage(attackDamage);
        }  
    }

    protected virtual void ResumeFromAttack()
    {
        StartCoroutine(Utils.Timeout(() =>
        {
            agent.isStopped = false;
        }, attackRate/2));
    }
}
