using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectTower
{
    public class MinionAttackState : MinionState
    {
        public MinionAttackState(GameObject owner, StateMachine stateMachine)
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
                if (attack.taunt)
                {
                    attack.ApplyTaunt();
                }
                attack.Attack();
            }
            else
            {
                stateMachine.ChangeState(attack.chaseState);
            }
        }
    }
}