﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectTower
{
    public class Minion : AttackBase
    {
        [SerializeField] public GameObject highlight;
        [HideInInspector] public bool taunt;
        [HideInInspector] public bool invisible;
        [HideInInspector] public bool guard;
        [HideInInspector] public bool charge;
        [HideInInspector] public bool ambush;


        [HideInInspector] public Vector3 guardPosition;
        [HideInInspector] public float guardRadius;
        [HideInInspector] public Vector3 initialPosition;

        [HideInInspector] public Card.Owner owner;
        [HideInInspector] public bool isSelected = false;

        private bool inCombat = true;
        private EffectManager effectManager;
        private LevelController levelController;

        public StateMachine stateMachine { get; protected set; }
        public MinionState idleState { get; protected set; }
        public MinionState chaseState { get; protected set; }
        public MinionState attackState { get; protected set; }
        public MinionState deathState { get; protected set; }
        public MinionState stunState { get; protected set; }

        public MinionState stageClearState { get; protected set; }

        // Start is called before the first frame update
        protected override void Start()
        {
            base.Start();
            layerMask = LayerMask.GetMask("Enemy");
            effectManager = FindObjectOfType<EffectManager>();
            levelController = FindObjectOfType<LevelController>();
            levelController.StageClear += OnStageClear;
            health.Death += OnDeath;

            stateMachine = new StateMachine();
            idleState = new MinionIdleState(gameObject, stateMachine);
            chaseState = new MinionChaseState(gameObject, stateMachine);
            attackState = new MinionAttackState(gameObject, stateMachine);
            deathState = new MinionDeathState(gameObject, stateMachine);
            stunState = new MinionStunState(gameObject, stateMachine);
            stageClearState = new MinionStageClearState(gameObject, stateMachine);
            stateMachine.Init(idleState);
    }

        // Update is called once per frame
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
                spriteRenderer.flipX = agent.velocity.x < -0.1;
            }
        }

        public void FreezeWalkAnimation()
        {
            animator.SetFloat("Velocity", 0);
        }

        public virtual void ReturnToPos()
        {
            if (Vector3.Distance(
                Vector3.ProjectOnPlane(transform.position, Vector3.up), 
                initialPosition) > 0.5f)
            {
                UpdateFlip();
                agent.isStopped = false;
                agent.stoppingDistance = 0;
                agent.SetDestination(initialPosition);
            }
            else
            {
                spriteRenderer.flipX = false;
                agent.isStopped = true;
            }
        }

        public override void AcquireTarget()
        {
            if (charge)
            {
                GetEnemies(gameObject.transform.position, 100f);
                SetTarget();
            }
            else if (guard)
            {
                GetEnemies(guardPosition, guardRadius + 0.5f);
                SetTarget();
            }
        }

        public override void ApplyTaunt()
        {
            if (enemiesInRange == null) return;
            foreach (Collider c in enemiesInRange)
            {
                effectManager.Taunt(gameObject, c.gameObject, 0);
            }
        }

        public virtual void Stun()
        {
            stateMachine.ChangeState(stunState);
        }

        public virtual void UnStun()
        {
            stateMachine.ChangeState(idleState);
        }

        protected override void NoTargetBehavior()
        {
            stateMachine.ChangeState(idleState);
        }


        protected virtual void OnStageClear(object sender, EventArgs e)
        {
            stateMachine.ChangeState(stageClearState);
        }

        protected virtual void OnDeath(object sender, EventArgs e)
        {
            stateMachine.ChangeState(deathState);
        }
    }
}