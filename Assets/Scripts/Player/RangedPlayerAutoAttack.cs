using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;
using Werewolf.StatusIndicators.Components;
using ProjectTower;

namespace ProjectTower
{
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

        private bool isCharging = false;

        protected Vector3 aimedPos;
        protected Vector3 lastTargetPos;

        protected override void Start()
        {
            base.Start();
            GetComponent<PlayerController>().StartWalking += OnInterrupt;
            GetComponent<PlayerController>().StartCasting += OnInterrupt;
            GetComponent<PlayerHealth>().Damaged += OnInterrupt;
            missiles = new List<Missile>();
        }

        public override bool TestRange()
        {
            if (agent.velocity.magnitude < Mathf.Epsilon
                && Vector3.Distance(
                    Vector3.ProjectOnPlane(agent.transform.position, new Vector3(0, 1, 0)),
                    Vector3.ProjectOnPlane(targetHitInfo.point, new Vector3(0, 1, 0)))
                    < stopRange + 1)
            {
                return true;
            }
            else
            {
                attackTimer = 1;
                return false;
            }
        }

        public override void Attack()
        {
            attackTimer -= Time.deltaTime;
            FlipX(targetHitInfo);
            if (!isSpecialing && !isCharging)
            {
                if (attackTimer < 0)
                {
                    isAttacking = true;
                    agent.isStopped = true;
                    animator.SetBool("Interrupt", false);
                    animator.SetTrigger("Attack");
                    audioManager.Play("Attack");
                    isCharging = true;
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
                    lastTargetPos = target.transform.position;
                    ToSummonMissiles(missileCount, rotation);
                }
            }
            if (!isAttacking)
            {
                SpeicalAttackUpdate();
            }
        }

        protected override void OnAttack(object sender, EventArgs e)
        {
            ResumeFromAttack();
            isCharging = false;
            attackTimer = attackRate;

            if (target == null)
            {
                SpecialAutoAttack();
                aimedPos = lastTargetPos;
                launchMissiles = StartCoroutine(LaunchMissiles());
                return;
            }

            var hitInfoArray = Physics.RaycastAll(gameObject.transform.position,
                target.transform.position - gameObject.transform.position, layerMask);

            if (hitInfoArray.Length == 0)
            {
                OnInterrupt(gameObject, EventArgs.Empty);
                return;
            }

            targetHitInfo = hitInfoArray.Where((h) => h.collider.gameObject == target)
                .FirstOrDefault();

            if (Vector3.Distance(
                    Vector3.ProjectOnPlane(agent.transform.position, new Vector3(0, 1, 0)),
                    Vector3.ProjectOnPlane(targetHitInfo.point, new Vector3(0, 1, 0)))
                    < stopRange + 2)
            {
                SpecialAutoAttack();
                aimedPos = targetHitInfo.point;
                launchMissiles = StartCoroutine(LaunchMissiles());
            }
            else
            {
                OnInterrupt(gameObject, EventArgs.Empty);
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
                ((GameObject)sender).GetComponent<Missile>().type);
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
            isCharging = false;
            attackTimer = attackRate;
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

            animator.SetBool("Interrupt", true);
        }

        protected virtual void ToSummonMissiles(int count, Quaternion rotation)
        {
            summonMissiles =
                StartCoroutine(SummonMissiles(count, rotation, 0));
        }

        protected IEnumerator SummonMissiles(int count, Quaternion rotation, int type)
        {
            int sign = 1;
            Quaternion massRotate;
            if (rotation == Quaternion.LookRotation(Vector3.right))
            {
                massRotate = Quaternion.Euler(20f, 35f, 0);
            }
            else
            {
                massRotate = Quaternion.Euler(20f, -35f, 0);
            }
            for (int i = 0; i < count; i++)
            {
                var angle = 50f / (count / 2) * (int)((i + 1) / 2);
                var position = massRotate
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
                m.HitEnemy += OnHitEnemy;
                m.OutOfRange += OnOutOfRange;
                m.enabled = false;
                missiles.Add(m);

                yield return new WaitForSeconds(readyTime / count);
            }
        }

        private IEnumerator LaunchMissiles()
        {
            var myMissiles = new List<Missile>(missiles);
            var count = myMissiles.Count;
            foreach (Missile m in myMissiles)
            {
                var direction = aimedPos - m.transform.position
                    + new Vector3(0, 2, 0);
                m.enabled = true;
                m.Launch(direction, missileSpeed);
                missiles.Remove(m);
                yield return new WaitForSeconds(0.8f / count);
            }
        }

        public override void RefreshTimer()
        {
            attackTimer = 1f;
        }
    }
}