using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Minion : MonoBehaviour
{
    [HideInInspector] public bool taunt;
    [HideInInspector] public bool invisible;
    [HideInInspector] public bool guard;
    [HideInInspector] public bool charge;
    [HideInInspector] public bool ambush;

    [HideInInspector] public int attackDamage;
    [HideInInspector] public float attackRange;
    [HideInInspector] public float attackRate;
    [HideInInspector] public float stopRange;

    [HideInInspector] public Vector3 guardPosition;
    [HideInInspector] public float guardRadius;
    [HideInInspector] public Vector3 initialPosition;

    [SerializeField] NavMeshAgent agent;
    [SerializeField] Animator animator;
    [SerializeField] SpriteRenderer spriteRenderer;

    private Collider[] enemiesInRange;
    private GameObject target;
    private int layerMask;
    private float attackTimer;

    // Start is called before the first frame update
    void Start()
    {
        agent.updateRotation = false;
        agent.updateUpAxis = false;
        layerMask = LayerMask.GetMask("Enemy");
    }

    // Update is called once per frame

    void Update()
    {
        if (charge)
        {
            attackRange = 10000;
            GetEnemies(gameObject.transform.position, attackRange);
            SetTarget();
            Attack();
        }

        if (guard)
        {
            GetEnemies(guardPosition, guardRadius);
            SetTarget();
            Attack();
            if (target == null)
            {
                agent.stoppingDistance = 0;
                agent.SetDestination(initialPosition);
            }
        }
    }

    private void GetEnemies(Vector3 source, float range)
    {
        enemiesInRange = Physics.OverlapSphere(
            source, range, layerMask);
    }

    private void SetTarget()
    {
        if (enemiesInRange.Length == 0)
        {
            target = null;
            return;
        }

        Collider closestEnemy = enemiesInRange[0];
        float min_dist = Mathf.Infinity;
        foreach (Collider e in enemiesInRange)
        {
            float dist = Vector3.Distance(gameObject.transform.position,
                                            e.transform.position);
            if (dist < min_dist)
            {
                closestEnemy = e;
                min_dist = dist;
            }
        }
        target = closestEnemy.gameObject;
    }

    private void Attack()
    {
        if (target == null) return;

        RaycastHit hitInfo;
        if (Physics.Linecast(gameObject.transform.position,
                                target.transform.position,
                                out hitInfo, layerMask))
        {
            agent.stoppingDistance = stopRange;
            agent.SetDestination(hitInfo.point);
        }

        if (agent.velocity.magnitude < Mathf.Epsilon
            && Vector3.Distance(
                Vector3.ProjectOnPlane(agent.transform.position, new Vector3(0, 1, 0)),
                Vector3.ProjectOnPlane(target.transform.position, new Vector3(0, 1, 0)))
                < stopRange + 2)
        {
            attackTimer -= Time.deltaTime;
            spriteRenderer.flipX =
               hitInfo.point.x < gameObject.transform.position.x;
            if (attackTimer < 0)
            {
                animator.SetTrigger("Attack");
                target.GetComponent<Health>()?.TakeDamage(attackDamage);
                attackTimer = attackRate;
            }
        }
        else
        {
            attackTimer = attackRate;
        }
    }
}
