using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public interface IState
{
    void Enter();
    void Update();
    void Exit();
}

public abstract class StateBase
{
    protected StateMachine stateMachine;
    protected BlackBoard blackBoard;

    public StateBase(StateMachine stateMachine, BlackBoard blackBoard)
    {
        this.stateMachine = stateMachine;
        this.blackBoard = blackBoard;
    }

    public abstract void Enter();
    public abstract void Update();
    public abstract void Exit();
}

public abstract class UnitStateBase : StateBase
{
    public UnitStateBase(UnitStateMachine stateMachine, BlackBoard blackBoard) : base(stateMachine, blackBoard) { }
}

public abstract class BuildingStateBase : StateBase
{
    public BuildingStateBase(BuildingStateMachine stateMachine, BlackBoard blackBoard) : base(stateMachine, blackBoard) { }
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
    public UnitIdleState    IdleState   { get; private set; }
    public UnitMoveState    MoveState   { get; private set; }
    public UnitAttackState  AttackState { get; private set; }

    public UnitStateMachine(NavMeshAgent agent, BlackBoard blackBoard)
    {
        IdleState   = new UnitIdleState(this, blackBoard, agent);
        MoveState   = new UnitMoveState(this, blackBoard, agent);
        AttackState = new UnitAttackState(this, blackBoard);
    }
}
public class BuildingStateMachine : StateMachine
{
    // States
    public UnitIdleState IdleState { get; private set; }

    public BuildingStateMachine(NavMeshAgent agent, BlackBoard blackBoard)
    {
        IdleState = new UnitIdleState(this, blackBoard, agent);
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