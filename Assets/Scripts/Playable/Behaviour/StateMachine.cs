using System;
using UnityEngine;
using UnityEngine.AI;

public abstract class StateBase
{
    public abstract void Enter();
    public abstract void Update();
    public abstract void Exit();
}

public abstract class UnitStateBase : StateBase
{
    protected UnitStateMachine stateMachine;
    protected BlackBoard blackBoard;

    public UnitStateBase(UnitStateMachine stateMachine, BlackBoard blackBoard)
    {
        this.stateMachine = stateMachine;
        this.blackBoard = blackBoard;
    }
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


public class UnitStateMachine : StateMachine
{
    // States
    public IdleState    IdleState   { get; private set; }
    public MoveState    MoveState   { get; private set; }
    public AttackState  AttackState { get; private set; }

    public UnitStateMachine(NavMeshAgent agent, BlackBoard blackBoard)
    {
        IdleState   = new IdleState(this, blackBoard, agent);
        MoveState   = new MoveState(this, blackBoard, agent);
        AttackState = new AttackState(this, blackBoard);
    }
}


public class BlackBoard
{
    public EntityData data;

    public Target target;

    public BlackBoard(EntityData data)
    {
        this.data = data;
    }
}