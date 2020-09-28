using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectTower
{
    public class EliteWolfIdleState : EliteWolfState
    {
        public EliteWolfIdleState(GameObject owner, StateMachine stateMachine)
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
            // start attack
            boss.AcquireTarget();
            if (boss.TestTarget())
            {
                stateMachine.ChangeState(boss.attackState);
            }
            else
            {
                boss.StompUpdate();
            }

            if (boss.health.currHealth <=
                (int)(boss.health.maxHealth * 0.25))
            {
                stateMachine.ChangeState(boss.enragedState);
            }
        }
    }
}