using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;
using Werewolf.StatusIndicators.Components;

public class PlayerAutoAttack : MonoBehaviour
{
    [SerializeField] int attackDamage = 20;
    [SerializeField] float attackRange;
    [SerializeField] float attackRate;
    [SerializeField] float stopRange;
    [SerializeField] NavMeshAgent agent;
    [SerializeField] Animator animator;
    [SerializeField] SpriteRenderer spriteRenderer;

    public SplatManager splat { get; set; }

    private PlayerController player;
    private Collider[] enemiesInRange;
    private GameObject target;
    private int layerMask;
    private float attackTimer;


    // Start is called before the first frame update
    void Start()
    {
        player = GetComponent<PlayerController>();
        layerMask = LayerMask.GetMask("Enemy");
        splat = GetComponentInChildren<SplatManager>();
        
        splat.SelectRangeIndicator(gameObject.name + "Range");
        splat.CurrentRangeIndicator.DefaultScale = (attackRange + 1) * 2;
        splat.CurrentRangeIndicator.Scale = (attackRange + 1) * 2;
    }

    // Update is called once per frame
    void Update()
    {
        GetEnemies();
        SetTarget();
        if (!player.isWalking && !player.isCasting)
        {
            Attack();
        }
    }

    private void GetEnemies()
    {
        enemiesInRange = Physics.OverlapSphere(
            gameObject.transform.position, attackRange, layerMask);
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

        if (agent.velocity.magnitude < Mathf.Epsilon)
        {
            attackTimer -= Time.deltaTime;
            spriteRenderer.flipX =
               hitInfo.point.x < gameObject.transform.position.x;
            if (attackTimer < 0)
            {
                animator.SetTrigger("Attack");
                if (gameObject.name == "Luban")
                {
                    GetComponent<LubanResource>().ResourceAutoGen();
                }
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
