using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectTower
{
    public class StateMachine
    {
        public State CurrentState { get; private set; }

        public void Init(State startingState)
        {
            CurrentState = startingState;
            CurrentState.Enter();
        }

        public void ChangeState(State newState)
        {
            CurrentState.Exit();
            CurrentState = newState;
            CurrentState.Enter();
        }
    }
}