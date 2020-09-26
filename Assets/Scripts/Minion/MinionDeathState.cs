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
            attack.agent.isStopped = true;
            attack.isSelected = false;
            attack.agent.destination = owner.transform.position;
        }

        public override void Exit()
        {
            base.Exit();
        }

        public override void LogicUpdate()
        {
            attack.FreezeWalkAnimation();
        }
    }
}