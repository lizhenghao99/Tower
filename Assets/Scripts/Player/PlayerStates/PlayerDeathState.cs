using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectTower
{
    public class PlayerDeathState : PlayerState
    {
        public PlayerDeathState(GameObject owner, StateMachine stateMachine)
            : base(owner, stateMachine) { }

        public override void Enter()
        {
            base.Enter();
            playerController.agent.SetDestination(owner.transform.position);
            playerController.agent.isStopped = true;
            playerController.isSelected = false;
            
        }

        public override void Exit()
        {
            base.Exit();
            playerController.agent.isStopped = false;
        }

        public override void LogicUpdate()
        {
            playerController.FreezeWalkAnimation();
            playerController.agent.isStopped = true;
        }
    }
}
