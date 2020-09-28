using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectTower
{
    public class EliteWolfState : EnemyState
    {
        protected EliteWolfAttack boss;
        public EliteWolfState(GameObject owner, StateMachine stateMachine)
            : base(owner, stateMachine)
        {
            boss = owner.GetComponent<EliteWolfAttack>();
        }
    }
}