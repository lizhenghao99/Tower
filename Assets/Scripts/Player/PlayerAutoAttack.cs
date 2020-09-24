using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;
using Werewolf.StatusIndicators.Components;

namespace ProjectTower
{
    public class PlayerAutoAttack : AttackBase
    {
        public SplatManager splat { get; set; }
        private PlayerController player;
        private LevelController levelController;

        // Start is called before the first frame update
        protected override void Start()
        {
            base.Start();
            layerMask = LayerMask.GetMask("Enemy");

            player = GetComponent<PlayerController>();
            splat = GetComponentInChildren<SplatManager>();

            levelController = FindObjectOfType<LevelController>();

            levelController.StartCombat += OnStartCombat;
            levelController.EndCombat += OnEndCombat;
            health.death += OnPlayerDeath;
            health.revive += OnPlayerRevive;
        }

        public void OnStartCombat(object sender, EventArgs e)
        {
            splat.SelectRangeIndicator(gameObject.name + "Range");
            splat.CurrentRangeIndicator.DefaultScale = (attackRange + 1) * 2;
            splat.CurrentRangeIndicator.Scale = (attackRange + 1) * 2;
        }

        public void OnEndCombat(object sender, EventArgs e)
        {
            splat.CancelRangeIndicator();
        }

        public void OnPlayerDeath(object sender, EventArgs e)
        {
            splat.CancelRangeIndicator();
        }

        public void OnPlayerRevive(object sender, EventArgs e)
        {
            splat.SelectRangeIndicator(gameObject.name + "Range");
            splat.CurrentRangeIndicator.DefaultScale = (attackRange + 1) * 2;
            splat.CurrentRangeIndicator.Scale = (attackRange + 1) * 2;
        }

        // Update is called once per frame
        protected override void Update()
        {
            if (health.isDead)
            {
                agent.destination = gameObject.transform.position;
                agent.isStopped = true;
                return;
            }
            GetEnemies(gameObject.transform.position, attackRange);
            SetTarget();
            if (!player.isWalking)
            {
                ApplyTaunt();
                if (!player.isCasting)
                {
                    Attack();
                }
            }
            else
            {
                if (player.isCasting)
                {
                    ApplyTaunt();
                }
            }
        }

        protected override bool TauntedBehavior()
        {
            return false;
        }

        protected override void NoTargetBehavior()
        {
            // do nothing
        }
    }
}