using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectTower
{
    public class PlayerIdleState : PlayerState
    {
        public PlayerIdleState(GameObject owner, StateMachine stateMachine)
            : base(owner, stateMachine) { }

        public override void Enter()
        {
            base.Enter();
            playerController.agent.isStopped = true;
        }

        public override void Exit()
        {
            base.Exit();
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();
            playerController.UpdateWalkAnimation();
            // start walk
            if (playerController.myWaypoint != null)
            {
                stateMachine.ChangeState(playerController.walkState);
                return;
            }
            // start chase
            playerAutoAttack.AcquireTarget();
            if (playerAutoAttack.TestTarget())
            {
                stateMachine.ChangeState(playerController.chaseState);
            }
        }
    }
}
