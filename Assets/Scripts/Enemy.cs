using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{

    [SerializeField] NavMeshAgent agent;
    [SerializeField] Animator animator;
    [SerializeField] SpriteRenderer spriteRenderer;
    [SerializeField] float stopRange;
    [SerializeField] int attackDamage;
    [SerializeField] float attackRate;

    private PlayerController[] players;
    private PlayerController target;

    private LayerMask layerMask;
    private float attackTimer;

    private void Awake()
    {
        gameObject.SetActive(false);
    }

    // Start is called before the first frame update
    void Start()
    {
        layerMask = LayerMask.GetMask("Player");
        players = FindObjectsOfType<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        SetTarget();
        Attack();
    }


    public void Spawn()
    {
        gameObject.SetActive(true);
    }  


    private void SetTarget()
    {
        PlayerController closestPlayer = players[0];
        float min_dist = Mathf.Infinity;
        foreach (PlayerController player in players)
        {
            float dist = Vector3.Distance(gameObject.transform.position,
                                            player.transform.position);
            if (dist < min_dist)
            {
                closestPlayer = player;
                min_dist = dist;
            }
        }
        target = closestPlayer;
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
                target.GetComponent<PlayerHealth>()?.ChangeHealth(-attackDamage);
                attackTimer = attackRate;
            }
        }
        else
        {
            attackTimer = attackRate;
        }
    }
}
