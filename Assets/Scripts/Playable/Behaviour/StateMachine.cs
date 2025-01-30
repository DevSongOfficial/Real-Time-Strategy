using UnityEngine;

public abstract class StateBase
{
    public abstract void Enter();
    public abstract void Update();
    public abstract void Exit();
}

public class StateMachine
{
    public StateBase CurrentState { get; private set; }

    public void ChangeState(StateBase newState)
    {
        CurrentState?.Exit();
        CurrentState = newState;
        CurrentState.Enter();
    }

    public void Update()
    {
        CurrentState?.Update();
    }

}