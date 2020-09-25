using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectTower
{
    public class PlayerAttackState : PlayerState
    {
        public PlayerAttackState(GameObject owner, StateMachine stateMachine)
            : base(owner, stateMachine) { }

        public override void Enter()
        {
            base.Enter();
            playerAutoAttack.RefreshTimer();
        }

        public override void Exit()
        {
            base.Exit();
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();
            playerController.UpdateWalkAnimation();

            // attack logic
            playerAutoAttack.AcquireTarget();
            if (playerAutoAttack.TestTarget() && playerAutoAttack.TestRange())
            {
                playerAutoAttack.ApplyTaunt();
                playerAutoAttack.Attack();
            }
            else
            {
                stateMachine.ChangeState(playerController.chaseState);
            }
        }
    }
}
