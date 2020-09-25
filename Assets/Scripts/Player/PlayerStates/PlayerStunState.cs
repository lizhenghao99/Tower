using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectTower
{
    public class PlayerStunState : PlayerState
    {
        public PlayerStunState(GameObject owner, StateMachine stateMachine)
            : base(owner, stateMachine) { }

        public override void Enter()
        {
            base.Enter();
            playerController.agent.SetDestination(owner.transform.position);
            playerController.agent.isStopped = true;
        }

        public override void Exit()
        {
            base.Exit();
            playerController.agent.isStopped = false;
        }

        public override void LogicUpdate()
        {
            playerController.FreezeWalkAnimation();
        }
    }
}
