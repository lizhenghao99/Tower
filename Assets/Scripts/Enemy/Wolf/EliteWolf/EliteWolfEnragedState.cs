using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectTower
{
    public class EliteWolfEnragedState : EliteWolfState
    {
        public EliteWolfEnragedState(GameObject owner, StateMachine stateMachine)
            : base(owner, stateMachine) { }

        public override void Enter()
        {
            base.Enter();
            boss.EnterRage();
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
                // do nothing
            }

            if (boss.health.currHealth <=
                (int)(boss.health.maxHealth * 0.1))
            {
                stateMachine.ChangeState(boss.wipeState);
            }
        }
    }
}