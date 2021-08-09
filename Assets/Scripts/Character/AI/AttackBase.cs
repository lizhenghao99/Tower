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
        [SerializeField] public NavMeshAgent agent;
        [SerializeField] protected Animator animator;
        [SerializeField] protected SpriteRenderer spriteRenderer;

        [HideInInspector] public GameObject taunter;

        protected Collider[] enemiesInRange;
        protected GameObject target;
        protected RaycastHit targetHitInfo;
        protected int layerMask;
        protected float attackTimer = 1;

        public Health health { get; protected set; }

        protected float meleeRange = 5f;

        protected bool isAttacking = false;
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
            attackEvent.Attack += OnAttack;
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
            float minDist = Mathf.Infinity;
            foreach (Collider e in enemiesInRange)
            {
                if (e.gameObject.GetComponent<Health>().isDead) continue;
                float dist = Vector3.Distance(gameObject.transform.position,
                                                e.transform.position);
                if (dist < minDist)
                {
                    closestEnemy = e;
                    minDist = dist;
                }
            }
            if (closestEnemy)
            {
                target = closestEnemy.gameObject;
            }
        }

        public virtual void AcquireTarget()
        {
            GetEnemies(gameObject.transform.position, attackRange);
            SetTarget();
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
                RaycastHit[] hitInfoArray;

                hitInfoArray = Physics.RaycastAll(gameObject.transform.position,
                    target.transform.position - gameObject.transform.position);

                if (hitInfoArray.Length == 0) return false;

                targetHitInfo = hitInfoArray.Where((h) => h.collider.gameObject == target)
                    .FirstOrDefault();
                return true;
            }
        }

        public virtual void Chase()
        {
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
                    isAttacking = true;
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
            if (!isAttacking)
            {
                SpeicalAttackUpdate();
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
            isAttacking = false;
            StartCoroutine(Utils.Timeout(() =>
            {
                if (agent != null && agent.isActiveAndEnabled)
                {
                    agent.isStopped = false;
                }
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