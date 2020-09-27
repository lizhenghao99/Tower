using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectTower
{
    public class MinionStunState : MinionState
    {
        public MinionStunState(GameObject owner, StateMachine stateMachine)
            : base(owner, stateMachine) { }

        public override void Enter()
        {
            base.Enter();
            attack.agent.SetDestination(owner.transform.position);
            attack.agent.isStopped = true;
        }

        public override void Exit()
        {
            base.Exit();
            attack.agent.isStopped = false;
        }

        public override void LogicUpdate()
        {
            attack.FreezeWalkAnimation();
        }
    }
}
