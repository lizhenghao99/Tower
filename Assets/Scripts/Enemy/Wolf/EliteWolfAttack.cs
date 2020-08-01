using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TowerUtils;
using System.Linq;
using Com.LuisPedroFonseca.ProCamera2D;
using UnityEngine.Audio;

public class EliteWolfAttack : EnemyAttack
{
    [SerializeField] GameObject stompFx;
    [SerializeField] GameObject[] spawnees;
    private float swipeTimer = 10f;
    private float howlTimer = 20f;
    private int spawnCounter = 0;
    private List<GameObject> spawnedObjects;
    private StompEvent stompEvent;
    private ProCamera2DShake shake;

    private int phase = 0;

    private int stompDamage = 100;
    private EffectManager effectManager;


    protected override void Start()
    {
        base.Start();
        stompEvent = GetComponentInChildren<StompEvent>();
        stompEvent.stomp += OnStomp;
        GetComponent<Health>().immuneStun = true;
        shake = Camera.main.GetComponent<ProCamera2DShake>();
        spawnedObjects = new List<GameObject>();
        effectManager = FindObjectOfType<EffectManager>();
    }

    protected override void Update()
    {
        base.Update();
        if (phase == 0 && health.currHealth <= health.maxHealth * 0.2)
        {
            EnterRage();
        }
        else if (phase == 1 && health.currHealth <= health.maxHealth * 0.05)
        {
            EnterWipe();
            health.isImmune = true;
        }
    }
    

    protected override void Attack()
    {
        if (target == null || !target.activeInHierarchy) return;

        RaycastHit[] hitInfoArray;

        hitInfoArray = Physics.RaycastAll(gameObject.transform.position,
            target.transform.position - gameObject.transform.position);

        if (hitInfoArray.Length == 0) return;

        targetHitInfo = hitInfoArray.Where((h) => h.collider.gameObject == target)
            .FirstOrDefault();

        if ((agent.velocity.magnitude < Mathf.Epsilon || stopRange < meleeRange)
            && Vector3.Distance(
                Vector3.ProjectOnPlane(agent.transform.position, new Vector3(0, 1, 0)),
                Vector3.ProjectOnPlane(targetHitInfo.point, new Vector3(0, 1, 0)))
                < stopRange + 2)
        {
            attackTimer -= Time.deltaTime;
            if (!isSpecialing)
            {
                if (attackTimer < 0)
                {
                    agent.isStopped = true;
                    animator.SetTrigger("Attack");
                    StartCoroutine(TowerUtils.Utils.Timeout(() =>
                    {
                        audioManager.Play("Attack");
                    }, 0.2f));
                    // attack behavior moved to animation
                    attackTimer = attackRate;
                }
            }
            SpeicalAttackUpdate();
        }
        else
        {
            attackTimer = attackRate;
            StompUpdate();
        }
    }

    private void StompUpdate()
    {
        swipeTimer -= Time.deltaTime;
        if (swipeTimer < 0)
        {
            isSpecialing = true;
            StartCoroutine(TowerUtils.Utils.Timeout(() =>
            {
                isSpecialing = false;
            }, 2f));
            animator.SetTrigger("Stomp");
            swipeTimer = 10f;
        }
    }

    protected override void SpeicalAttackUpdate()
    {
        howlTimer -= Time.deltaTime;
        if (howlTimer < 0)
        {
            isSpecialing = true;
            StartCoroutine(TowerUtils.Utils.Timeout(() =>
            {
                isSpecialing = false;
            }, 3f));
            animator.SetTrigger("Howl");
            Howl();
            howlTimer = 20f;
        }
    }

    private void Howl()
    {
        int index = spawnCounter % spawnees.Length;
        for (int i = 0; i < 3; i++)
        {
            var spawned = Instantiate(spawnees[index+i], 
                gameObject.transform, true);
            spawned.SetActive(true);
            spawnedObjects.Add(spawned);
        }
        spawnCounter += 3;

        foreach (GameObject e in spawnedObjects)
        {
            if (e.activeInHierarchy && !e.GetComponent<Health>().isDead)
            {
                effectManager.Register(gameObject, e,
                    Effect.Type.Rage, 10f, 0.25f);
            }
        }

        shake.Shake("EliteWolfHowlShake");
        audioManager.Play("Howl");
    }

    protected override void SpecialAutoAttack()
    {
        var temp = layerMask;
        layerMask = LayerMask.GetMask("Player", "Minion", "Base");
        GetEnemies(target.transform.position, 10f);
        foreach (Collider enemy in enemiesInRange)
        {
            enemy.gameObject.GetComponent<Health>()?.TakeDamage(attackDamage / 3);
        }
        layerMask = temp;
    }

    protected void OnStomp(object sender, EventArgs e)
    {
        var stompRange = 60f;

        var fx = Instantiate(stompFx, gameObject.transform);
        fx.transform.localScale = new Vector3(stompRange / 2, stompRange / 2, 0.5f);

        audioManager.Play("Stomp");
        shake.Shake("EliteWolfStompShake");


        var swipeMask = LayerMask.GetMask("Player", "Minion", "Base");
        Collider[] colliders = Physics.OverlapSphere(
            gameObject.transform.position, stompRange, swipeMask);
        foreach (Collider c in colliders)
        {
            if (c.gameObject.CompareTag("Minion")
                && !c.gameObject.GetComponent<Minion>().taunt)
            {
                var direction =
                    Vector3.ProjectOnPlane(
                        c.transform.position - gameObject.transform.position,
                        new Vector3(0, 1, 0)).normalized;

                c.GetComponent<Rigidbody>().AddForce(
                    direction * 30, ForceMode.VelocityChange);
            }
            if (c.gameObject.CompareTag("Player"))
            {
                var direction =
                    Vector3.ProjectOnPlane(
                        c.transform.position - gameObject.transform.position,
                        new Vector3(0, 1, 0)).normalized;

                c.GetComponent<Rigidbody>().AddForce(
                    direction * 15, ForceMode.VelocityChange);
            }
            c.GetComponent<Health>().TakeDamage(stompDamage);
        }

        ResumeFromAttack();
    }
    private void EnterRage()
    {
        phase = 1;
        isSpecialing = true;
        StartCoroutine(TowerUtils.Utils.Timeout(() =>
        {
            isSpecialing = false;
        }, 3f));
        animator.SetTrigger("Howl");

        foreach (GameObject e in spawnees)
        {
            var spawned = Instantiate(e,
                gameObject.transform, true);
            spawned.SetActive(true);
            spawnedObjects.Add(spawned);
        }

        foreach (GameObject e in spawnedObjects)
        {
            if (e.activeInHierarchy && !e.GetComponent<Health>().isDead)
            {
                effectManager.Register(gameObject, e,
                    Effect.Type.Rage, 500f, 2f);
            }
        }

        shake.Shake("EliteWolfHowlShake");
        audioManager.Play("Howl");

        effectManager.Register(gameObject, gameObject,
            Effect.Type.Rage, 500f, 0.5f);
    }

    private void EnterWipe()
    {
        phase = 2;
        isSpecialing = true;
        stompDamage = 10000;
        animator.SetTrigger("Stomp");
        enabled = false;
    }
}
