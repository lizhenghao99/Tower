using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ProjectTower;

namespace ProjectTower
{
    public class GreyWolfAttack : EnemyAttack
    {
        [SerializeField] GameObject stompFx;
        private float swipeTimer = 5f;
        private StompEvent stompEvent;

        protected override void Start()
        {
            base.Start();
            stompEvent = GetComponentInChildren<StompEvent>();
            stompEvent.stomp += OnStomp;
        }

        protected override void SpeicalAttackUpdate()
        {
            swipeTimer -= Time.deltaTime;
            if (swipeTimer < 0)
            {
                isSpecialing = true;
                StartCoroutine(Utils.Timeout(() =>
                {
                    isSpecialing = false;
                }, 2f));
                animator.SetTrigger("Stomp");
                swipeTimer = 5f;
            }
        }

        protected void OnStomp(object sender, EventArgs e)
        {
            var stompRange = attackRange * 1.5f;

            var fx = Instantiate(stompFx, gameObject.transform);
            fx.transform.localScale = new Vector3(stompRange / 2, stompRange / 2, 0.5f);

            audioManager.Play("Stomp");


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
                        direction * 20, ForceMode.VelocityChange);
                }
                c.GetComponent<Health>().TakeDamage(attackDamage * 2);
            }

            ResumeFromAttack();
        }
    }
}