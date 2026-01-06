using UnityEngine;

public interface IState
{
    void Enter();
    void Update();
    void Exit();
}

public abstract class StateBase<T> : IState where T : StateMachineBase
{
    protected T stateMachine;
    protected BlackBoard blackBoard;

    public StateBase(T stateMachine, BlackBoard blackBoard)
    {
        this.stateMachine = stateMachine;
        this.blackBoard = blackBoard;
    }

    public abstract void Enter();
    public abstract void Update();
    public abstract void Exit();
}

public abstract class UnitStateBase : StateBase<UnitStateMachine>
{
    public UnitStateBase(UnitStateMachine stateMachine, BlackBoard blackBoard) : base(stateMachine, blackBoard) { }

    public override void Update()
    {
        if(blackBoard.attackCooldown > 0)
            blackBoard.attackCooldown -= Time.deltaTime;
    }
}

public abstract class BuildingStateBase : StateBase<BuildingStateMachine>
{
    public BuildingStateBase(BuildingStateMachine stateMachine, BlackBoard blackBoard) : base(stateMachine, blackBoard) { }
}