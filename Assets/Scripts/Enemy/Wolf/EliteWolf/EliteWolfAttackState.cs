using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectTower
{
    public class EliteWolfAttackState : EliteWolfState
    {
        public EliteWolfAttackState(GameObject owner, StateMachine stateMachine)
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

            // attack logic
            attack.AcquireTarget();
            if (attack.TestTarget() && attack.TestRange())
            {
                attack.Attack();
            }
            else
            {
                stateMachine.ChangeState(attack.idleState);
            }

            if (boss.health.currHealth <=
                (int)(boss.health.maxHealth * 0.25))
            {
                stateMachine.ChangeState(boss.enragedState);
            }
        }
    }
}