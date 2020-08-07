﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;
using Werewolf.StatusIndicators.Components;
using TowerUtils;

public class RangedPlayerAutoAttack : PlayerAutoAttack
{
    [Header("Projectile")]
    [SerializeField] public int missileCount;
    [SerializeField] float missileBlastRange;
    [SerializeField] Missile[] missilePrefabs;
    [SerializeField] float missileHeight;
    [SerializeField] float missileSpeed;
    [SerializeField] float missileRange;
    [SerializeField] protected float readyTime = 3f;

    protected List<Missile> missiles;
    protected Coroutine summonMissiles;
    protected Coroutine launchMissiles;

    protected override void Start()
    {
        base.Start();
        GetComponent<PlayerController>().startWalking += OnInterrupt;
        GetComponent<PlayerController>().startCasting += OnInterrupt;
        GetComponent<PlayerHealth>().damaged += OnInterrupt;
        missiles = new List<Missile>();
    }

    protected override void Attack()
    {
        if (target == null || !target.activeInHierarchy)
        {
            NoTargetBehavior();
            return;
        }

        RaycastHit[] hitInfoArray;

        hitInfoArray = Physics.RaycastAll(gameObject.transform.position,
            target.transform.position - gameObject.transform.position);

        if (hitInfoArray.Length == 0) return;

        targetHitInfo = hitInfoArray.Where((h) => h.collider.gameObject == target)
            .FirstOrDefault();
        agent.stoppingDistance = stopRange;
        agent.SetDestination(targetHitInfo.point);

        if ((agent.velocity.magnitude < Mathf.Epsilon || stopRange < meleeRange)
            && Vector3.Distance(
                Vector3.ProjectOnPlane(agent.transform.position, new Vector3(0, 1, 0)),
                Vector3.ProjectOnPlane(targetHitInfo.point, new Vector3(0, 1, 0)))
                < stopRange + 2)
        {
            attackTimer -= Time.deltaTime;
            FlipX(targetHitInfo);
            if (!isSpecialing)
            {
                if (attackTimer < 0)
                {
                    agent.isStopped = true;
                    animator.SetTrigger("Attack");
                    StartCoroutine(Utils.Timeout(() =>
                    {
                        audioManager.Play("Attack");
                    }, 0.2f));
                    // missle
                    Quaternion rotation;
                    if (targetHitInfo.point.x > transform.position.x)
                    {
                        rotation = Quaternion.LookRotation(Vector3.right);
                    }
                    else
                    {
                        rotation = Quaternion.LookRotation(Vector3.left);
                    }

                    ToSummonMissiles(missileCount, rotation);
                }
            }
            SpeicalAttackUpdate();
        }
        else
        {
            attackTimer = 1;
        }
    }

    protected override void OnAttack(object sender, EventArgs e)
    {
        ResumeFromAttack();

        if (target == null) return;

        var hitInfoArray = Physics.RaycastAll(gameObject.transform.position,
            target.transform.position - gameObject.transform.position);

        if (hitInfoArray.Length == 0) return;

        targetHitInfo = hitInfoArray.Where((h) => h.collider.gameObject == target)
            .FirstOrDefault();

        if (Vector3.Distance(
                Vector3.ProjectOnPlane(agent.transform.position, new Vector3(0, 1, 0)),
                Vector3.ProjectOnPlane(targetHitInfo.point, new Vector3(0, 1, 0)))
                < stopRange + 2)
        {
            SpecialAutoAttack();

            launchMissiles = StartCoroutine(LaunchMissiles());
        }
    }

    private void OnHitEnemy(object sender, Vector3 position)
    {
        missiles.Remove(((GameObject)sender).GetComponent<Missile>());
        Collider[] hits = 
            Physics.OverlapSphere(position, missileBlastRange, layerMask);
        foreach (Collider c in hits)
        {
            c.gameObject.GetComponent<Health>().TakeDamage(attackDamage);
        }
        SpecialRangedAttack(hits, 
            ((GameObject) sender).GetComponent<Missile>().type);
    }

    protected virtual void SpecialRangedAttack(Collider[] hits)
    {
        // do nothing
    }

    protected virtual void SpecialRangedAttack(Collider[] hits, int type)
    {
        // do nothing
    }

    private void OnOutOfRange(object sender, EventArgs e)
    {
        missiles.Remove(((GameObject)sender).GetComponent<Missile>());
    }

    private void OnInterrupt(object sender, EventArgs e)
    {
        if (summonMissiles != null)
        {
            StopCoroutine(summonMissiles);
        }
        if (launchMissiles != null)
        {
            StopCoroutine(launchMissiles);
        }
        
        foreach (Missile m in missiles)
        {
            m.Cancel();
        }
        missiles.Clear();
    }

    protected virtual void ToSummonMissiles(int count, Quaternion rotation)
    {
        summonMissiles = 
            StartCoroutine(SummonMissiles(count, rotation, 0));
    }

    protected IEnumerator SummonMissiles(int count, Quaternion rotation, int type)
    {
        int sign = 1;
        for (int i = 0; i < count; i++)
        {
            var angle = 50f / (count / 2) * (int)((i+1)/2);
            var position = Quaternion.Euler(20f, 35f, 0)
                * Quaternion.Euler(angle * sign, 0, 0)
                * new Vector3(0,
                    missileHeight * Mathf.Pow(0.75f, (int)((i + 1) / 2)),
                    0)
                + transform.position;
            sign *= -1;

            var m = Instantiate(missilePrefabs[type],
                        position,
                        rotation);
            m.range = missileRange;
            m.type = type;
            m.hitEnemy += OnHitEnemy;
            m.outOfRange += OnOutOfRange;
            m.enabled = false;
            missiles.Add(m);
            attackTimer = attackRate;
            yield return new WaitForSeconds(readyTime/missileCount);
        }
    }

    private IEnumerator LaunchMissiles()
    {
        var myMissiles = new List<Missile>(missiles);
        foreach (Missile m in myMissiles)
        {
            var direction = targetHitInfo.point - m.transform.position
                + new Vector3(0, 2, 0);
            m.enabled = true;
            m.Launch(direction, missileSpeed);
            missiles.Remove(m);
            yield return new WaitForSeconds(0.2f);
        }
    }
}
