using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;
using ProjectTower;

namespace ProjectTower
{
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
        protected RaycastHit targetHitInfo;
        protected int layerMask;
        protected float attackTimer = 1;

        protected Health health;

        protected float meleeRange = 5f;

        protected bool isSpecialing = false;
        protected AttackEvent attackEvent;

        protected InstanceAudioManager audioManager;

        // Start is called before the first frame update
        protected virtual void Start()
        {
            agent.updateRotation = false;
            agent.updateUpAxis = false;
            health = GetComponent<Health>();
            attackEvent = GetComponentInChildren<AttackEvent>();
            attackEvent.attack += OnAttack;
            audioManager = GetComponent<InstanceAudioManager>();
            if (audioManager == null)
            {
                audioManager = gameObject.AddComponent<InstanceAudioManager>();
            }
        }

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

        public virtual void ApplyTaunt()
        {
            // do nothing
        }

        public virtual bool TestTarget()
        {
            if (target == null || !target.activeInHierarchy)
            {
                NoTargetBehavior();
                return false;
            }
            else
            {
                return true;
            }
        }

        public virtual void Chase()
        {
            RaycastHit[] hitInfoArray;

            hitInfoArray = Physics.RaycastAll(gameObject.transform.position,
                target.transform.position - gameObject.transform.position);

            if (hitInfoArray.Length == 0) return;

            targetHitInfo = hitInfoArray.Where((h) => h.collider.gameObject == target)
                .FirstOrDefault();
            agent.stoppingDistance = stopRange;
            agent.SetDestination(targetHitInfo.point);
        }

        public virtual bool EnteredRange()
        {
            if ((agent.velocity.magnitude < Mathf.Epsilon || stopRange < meleeRange)
                && Vector3.Distance(
                    Vector3.ProjectOnPlane(agent.transform.position, new Vector3(0, 1, 0)),
                    Vector3.ProjectOnPlane(targetHitInfo.point, new Vector3(0, 1, 0)))
                    < stopRange + 0.5)
            {
                return true;
            }
            else
            {
                attackTimer = attackRate;
                return false;
            }
        }

        public virtual bool TestRange()
        {
            if ((agent.velocity.magnitude < Mathf.Epsilon || stopRange < meleeRange)
                && Vector3.Distance(
                    Vector3.ProjectOnPlane(agent.transform.position, new Vector3(0, 1, 0)),
                    Vector3.ProjectOnPlane(targetHitInfo.point, new Vector3(0, 1, 0)))
                    < stopRange + 1)
            {
                return true;
            }
            else
            {
                attackTimer = attackRate;
                return false;
            }
        }

        public virtual void Attack()
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
                    // attack behavior moved to animation
                    attackTimer = attackRate;
                }
            }
            SpeicalAttackUpdate();
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
                    Vector3.ProjectOnPlane(targetHitInfo.point, new Vector3(0, 1, 0)))
                    < stopRange + 2)
            {
                SpecialAutoAttack();
                target.GetComponent<Health>()?.TakeDamage(attackDamage);
                audioManager.Play("Hit");
            }
        }

        protected virtual void ResumeFromAttack()
        {
            StartCoroutine(Utils.Timeout(() =>
            {
                agent.isStopped = false;
            }, attackRate / 2));
        }

        protected virtual void NoTargetBehavior()
        {
            agent.SetDestination(gameObject.transform.position);
        }

        public virtual void RefreshTimer()
        {
            attackTimer = attackRate;
        }
    }
}