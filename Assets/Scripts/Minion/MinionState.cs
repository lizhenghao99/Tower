using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace ProjectTower
{
    public class MinionState : State
    {
        protected Minion attack;

        public MinionState(GameObject owner, StateMachine stateMachine)
            : base(owner, stateMachine)
        {
            attack = owner.GetComponent<Minion>();
        }

        public override void Enter()
        {
            // do nothing
        }

        public override void Exit()
        {
            // do nothing
        }

        public override void LogicUpdate()
        {
            if (attack.isSelected)
            {
                attack.highlight.SetActive(true);
            }
            else
            {
                attack.highlight.SetActive(false);
            }
        }
    }
}