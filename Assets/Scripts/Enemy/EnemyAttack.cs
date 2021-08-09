using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

namespace ProjectTower
{
    public class EnemyAttack : AttackBase
    {
        [SerializeField] public GameObject highlight;
        public bool isSelected = false;

        public StateMachine stateMachine { get; protected set; }

        public EnemyState idleState { get; protected set; }
        public EnemyState chaseState { get; protected set; }
        public EnemyState attackState { get; protected set; }
        public EnemyState deathState { get; protected set; }
        public EnemyState stunState { get; protected set; }

        // Start is called before the first frame update
        protected override void Start()
        {
            base.Start();
            layerMask = LayerMask.GetMask("BaseTower");
            health.Death += OnDeath;

            stateMachine = new StateMachine();
            idleState = new EnemyIdleState(gameObject, stateMachine);
            chaseState = new EnemyChaseState(gameObject, stateMachine);
            attackState = new EnemyAttackState(gameObject, stateMachine);
            deathState = new EnemyDeathState(gameObject, stateMachine);
            stunState = new EnemyStunState(gameObject, stateMachine);
            stateMachine.Init(idleState);
        }

        private void Update()
        {
            stateMachine.currentState.LogicUpdate();
        }

        public virtual void UpdateWalkAnimation()
        {
            animator.SetFloat("Velocity", agent.velocity.magnitude);
        }

        public virtual void UpdateFlip()
        {
            if (agent.velocity.magnitude > 0.1f)
            {
                spriteRenderer.flipX = agent.velocity.x > 0.1f;
            }
        }

        public virtual void FreezeWalkAnimation()
        {
            animator.SetFloat("Velocity", 0f);
        }

        public virtual void Stun()
        {
            stateMachine.ChangeState(stunState);
        }

        public virtual void Unstun()
        {
            stateMachine.ChangeState(idleState);
        }

        public override void AcquireTarget()
        {
            GetEnemies(gameObject.transform.position, 100);
            SetTarget();
        }

        protected override void FlipX(RaycastHit hitInfo)
        {
            spriteRenderer.flipX =
                   hitInfo.point.x > gameObject.transform.position.x;
        }

        protected virtual void OnDeath(object sender, EventArgs e)
        {
            stateMachine.ChangeState(deathState);
        }
    }
}
