using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectTower
{
    public class PlayerChaseState : PlayerState
    {
        public PlayerChaseState(GameObject owner, StateMachine stateMachine)
            : base(owner, stateMachine) { }

        public override void Enter()
        {
            base.Enter();
            playerController.agent.isStopped = false;
        }

        public override void Exit()
        {
            base.Exit();
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();
            playerController.UpdateWalkAnimation();
            playerController.UpdateFlip();

            // attack logic
            playerAutoAttack.AcquireTarget();
            
            if (playerAutoAttack.TestTarget())
            {
                playerAutoAttack.Chase();
                playerAutoAttack.ApplyTaunt();
                if (playerAutoAttack.EnteredRange())
                {
                    stateMachine.ChangeState(playerController.attackState);
                }
            }
            else
            {
                stateMachine.ChangeState(playerController.idleState);
            }
        }
    }
}