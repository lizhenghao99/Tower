using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectTower
{
    public class EnemyAttackState : EnemyState
    {
        public EnemyAttackState(GameObject owner, StateMachine stateMachine)
            : base(owner, stateMachine) { }

        public override void Enter()
        {
            base.Enter();
            attack.RefreshTimer();
        }

        public override void Exit()
        {
            base.Exit();
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();
            attack.UpdateWalkAnimation();

            // attack logic
            attack.AcquireTarget();
            if (attack.TestTarget() && attack.TestRange())
            {
                attack.Attack();
            }
            else
            {
                stateMachine.ChangeState(attack.chaseState);
            }
        }
    }
}