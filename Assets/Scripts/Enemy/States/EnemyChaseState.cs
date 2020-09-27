using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectTower
{
    public class EnemyChaseState : EnemyState
    {
        public EnemyChaseState(GameObject owner, StateMachine stateMachine)
            : base(owner, stateMachine) { }

        public override void Enter()
        {
            base.Enter();
        }

        public override void Exit()
        {
            base.Exit();
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();
            //animation
            attack.UpdateWalkAnimation();
            attack.UpdateFlip();
            // attack logic
            attack.AcquireTarget();
            if (attack.TestTarget())
            {
                attack.Chase();
                if (attack.EnteredRange())
                {
                    stateMachine.ChangeState(attack.attackState);
                }
            }
            else
            {
                stateMachine.ChangeState(attack.idleState);
            }
        }
    }
}