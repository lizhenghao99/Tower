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
            playerController.isSelected = false;
            playerController.agent.destination = owner.transform.position;
        }

        public override void Exit()
        {
            base.Exit();
        }

        public override void LogicUpdate()
        {
            playerController.FreezeWalkAnimation();
        }
    }
}
