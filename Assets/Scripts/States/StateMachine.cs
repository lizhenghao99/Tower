namespace ProjectTower
{
    public class StateMachine
    {
        public State currentState { get; private set; }

        public void Init(State startingState)
        {
            currentState = startingState;
            currentState.Enter();
        }

        public void ChangeState(State newState)
        {
            currentState.Exit();
            currentState = newState;
            currentState.Enter();
        }
    }
}