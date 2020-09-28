using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectTower
{
    public class MinionDeathState : MinionState
    {
        public MinionDeathState(GameObject owner, StateMachine stateMachine)
            : base(owner, stateMachine) { }

        public override void Enter()
        {
            base.Enter();
            attack.agent.SetDestination(owner.transform.position);
            attack.agent.isStopped = true;
            attack.isSelected = false;
        }

        public override void Exit()
        {
            base.Exit();
        }

        public override void LogicUpdate()
        {
            attack.FreezeWalkAnimation();
            attack.agent.isStopped = true;
        }
    }
}