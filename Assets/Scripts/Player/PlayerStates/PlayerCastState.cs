using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectTower
{
    public class PlayerCastState : PlayerState
    {
        public PlayerCastState(GameObject owner, StateMachine stateMachine)
            : base(owner, stateMachine) { }

        public override void Enter()
        {
            base.Enter();
            playerController.OnStartCasting();
            playerController.agent.isStopped = true;
        }

        public override void Exit()
        {
            base.Exit();
            playerController.OnFinishCasting();
            playerController.agent.isStopped = false;
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();
            playerController.FreezeWalkAnimation();
            playerAutoAttack.AcquireTarget();
            playerAutoAttack.ApplyTaunt();
        }
    }
}
