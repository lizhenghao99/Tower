using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectTower
{
    public class EnemyIdleState : EnemyState
    {
        public EnemyIdleState(GameObject owner, StateMachine stateMachine)
            : base(owner, stateMachine) { }

        public override void Enter()
        {
            base.Enter();
            if (attack.agent != null && attack.agent.isActiveAndEnabled)
            {
                attack.agent.isStopped = true;
            }
        }

        public override void Exit()
        {
            base.Exit();
            if (attack.agent != null && attack.agent.isActiveAndEnabled)
            {
                attack.agent.isStopped = false;
            }
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();
            // animation
            attack.UpdateWalkAnimation();
            // start chase
            attack.AcquireTarget();
            if (attack.TestTarget())
            {
                stateMachine.ChangeState(attack.chaseState);
            }
        }
    }
}