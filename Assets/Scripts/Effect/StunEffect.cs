using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace ProjectTower
{
    public class StunEffect : Effect
    {
        private PlayerController player;
        private Minion minion;
        private EnemyAttack enemy;

        private void Awake()
        {
            player = GetComponent<PlayerController>();
            minion = GetComponent<Minion>();
            enemy = GetComponent<EnemyAttack>();
        }

        protected override void OnStart()
        {
            var jin = GetComponent<LightningEffect>();
            if (jin != null)
            {
                jin.Enhance();
            }


            if (!GetComponent<Health>().immuneStun)
            {
                if (player != null)
                {
                    player.Stun();
                }
                else if (minion != null)
                {
                    minion.Stun();
                }
                else if (enemy != null)
                {
                    enemy.Stun();
                }
                base.OnStart();
            }
            else
            {
                OnFinish();
            }
        }

        protected override void OnFinish()
        {
            if (player != null)
            {
                player.UnStun();
            }
            else if (minion != null)
            {
                minion.UnStun();
            }
            else if (enemy != null)
            {
                enemy.Unstun();
            }
            base.OnFinish();
        }
    }
}