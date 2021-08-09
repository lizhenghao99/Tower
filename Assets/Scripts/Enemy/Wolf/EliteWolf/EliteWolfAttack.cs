using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using ProjectTower;
using System.Linq;
using Com.LuisPedroFonseca.ProCamera2D;
using UnityEngine.Audio;

namespace ProjectTower
{
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

        public EnemyState enragedState { get; protected set; }
        public EnemyState wipeState { get; protected set; }

        protected override void Start()
        {
            base.Start();
            stompEvent = GetComponentInChildren<StompEvent>();
            stompEvent.Stomp += OnStomp;
            GetComponent<Health>().immuneStun = true;
            shake = Camera.main.GetComponent<ProCamera2DShake>();
            spawnedObjects = new List<GameObject>();
            effectManager = FindObjectOfType<EffectManager>();

            idleState = new EliteWolfIdleState(gameObject, stateMachine);
            attackState = new EliteWolfAttackState(gameObject, stateMachine);
            enragedState = new EliteWolfEnragedState(gameObject, stateMachine);
            wipeState = new EliteWolfWipeState(gameObject, stateMachine);

            stateMachine.Init(idleState);
        }

        public override void AcquireTarget()
        {
            GetEnemies(transform.position, stopRange);
            SetTarget();
        }

        protected void OldUpdate()
        {
            if (phase == 0 && health.currHealth <=
                (int)(health.maxHealth * 0.25))
            {
                EnterRage();
            }
            else if (phase == 1 && health.currHealth <=
                (int)(health.maxHealth * 0.1))
            {
                EnterWipe();
            }
        }

        public void StompUpdate()
        {
            swipeTimer -= Time.deltaTime;
            if (swipeTimer < 0)
            {
                isSpecialing = true;
                StartCoroutine(ProjectTower.Utils.Timeout(() =>
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
                StartCoroutine(ProjectTower.Utils.Timeout(() =>
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
                var spawned = Instantiate(spawnees[index + i],
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
            layerMask = LayerMask.GetMask("Player", "Minion", "BaseTower");
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


            var swipeMask = LayerMask.GetMask("Player", "Minion", "BaseTower");
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
        public void EnterRage()
        {
            isSpecialing = true;
            StartCoroutine(ProjectTower.Utils.Timeout(() =>
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

        public void EnterWipe()
        {
            health.isImmune = true;
            isSpecialing = true;
            stompDamage = 10000;
            animator.SetTrigger("Stomp");
        }

        protected override void FlipX(RaycastHit hitInfo)
        {
            spriteRenderer.flipX = false;
        }
    }
}