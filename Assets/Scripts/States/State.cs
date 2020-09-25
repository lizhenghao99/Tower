using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectTower
{
    public abstract class State 
    {
        protected GameObject owner;
        protected StateMachine stateMachine;

        protected State(GameObject owner, StateMachine stateMachine)
        {
            this.owner = owner;
            this.stateMachine = stateMachine;
        }

        public abstract void Enter();

        public abstract void Exit();

        public abstract void LogicUpdate();
    }
}