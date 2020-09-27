using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace ProjectTower
{
    public class EnemyState : State
    {
        protected EnemyAttack attack;

        public EnemyState(GameObject owner, StateMachine stateMachine)
            : base(owner, stateMachine)
        {
            attack = owner.GetComponent<EnemyAttack>();
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