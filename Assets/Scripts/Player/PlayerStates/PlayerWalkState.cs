using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectTower
{
    public class PlayerWalkState : PlayerState
    {
        public PlayerWalkState(GameObject owner, StateMachine stateMachine)
            : base(owner, stateMachine) { }

        public override void Enter()
        {
            base.Enter();
            playerController.agent.isStopped = false;
            playerController.agent.stoppingDistance = 0;
            playerController.agent.SetDestination(
               Vector3.ProjectOnPlane(
                   playerController.myWaypoint.transform.position,
                   new Vector3(0,1,0)));
        }

        public override void Exit()
        {
            base.Exit();
            playerController.agent.isStopped = true;
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();
            // animation
            playerController.UpdateWalkAnimation();
            playerController.UpdateFlip();
            // walk auto stop
            playerController.WalkAutoStop();
        }
    }
}

