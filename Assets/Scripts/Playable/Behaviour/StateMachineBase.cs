using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.AppUI.Core;
using Unity.VisualScripting;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;

public abstract class StateMachineBase
{
    private readonly Dictionary<Type, IState> states = new Dictionary<Type, IState>();

    public IState CurrentState { get; private set; }
    public IState PreviousState { get; private set; }

    public void ChangeState<T>() where T : IState 
    { 
        ChangeState(states[typeof(T)]); 
    }

    protected void RegisterState(IState state) 
    { 
        states[state.GetType()] = state; 
    }

    private void ChangeState(IState newState)
    {
        if(newState == null) return;

        if(CurrentState != null)
        {
            PreviousState = CurrentState;
            CurrentState.Exit();
        }

        CurrentState = newState;
        CurrentState.Enter();
    }

    public void Update()
    {
        CurrentState?.Update();
    }
}

public sealed class UnitStateMachine : StateMachineBase
{
    public UnitIdleState IdleState { get; private set; }
    public UnitMoveState MoveState { get; private set; }
    public UnitAttackState AttackState { get; private set; }
    public UnitConstructState ConstructState { get; private set; }

    private BlackBoard blackBoard;

    public UnitStateMachine(IUnitStateContext stateContext, BlackBoard blackBoard)
    {
        this.blackBoard = blackBoard;

        IdleState = new UnitIdleState(this, blackBoard, stateContext);
        MoveState = new UnitMoveState(this, blackBoard, stateContext);
        AttackState = new UnitAttackState(this, blackBoard, stateContext);
        ConstructState = new UnitConstructState(this, blackBoard, stateContext);

        RegisterState(IdleState);
        RegisterState(MoveState);
        RegisterState(AttackState);
        RegisterState(ConstructState);

        // Initial State
        ChangeState<UnitIdleState>();
    }

    public UnitStateBase DetermineNextState()
    {
        Target target = blackBoard.target;

        if (target.IsGround)
            return IdleState;

        if (target.Entity.GetTeam() != blackBoard.team)
        {
            if (target.Entity is IDamageable)
                return AttackState;
        }
        else if (target.Entity is Building)
            return ConstructState;

        return IdleState;
    }
}

public class BuildingStateMachine : StateMachineBase
{
    public BuildingStateMachine(BlackBoard blackBoard)
    {
        RegisterState(new BuildingIdleState(this, blackBoard));

        ChangeState<BuildingIdleState>();
    }
}

public class HybridBuildingStateMachine : BuildingStateMachine
{
    public HybridBuildingStateMachine(BlackBoard blackBoard, NavMeshAgent agent) : base(blackBoard)
    {
        RegisterState(new BuildingMoveState(this, blackBoard, agent));
    }
}


public class BlackBoard
{
    public EntityData BaseData { get; private set; }

    public Team team;

    // Move
    public Target target;

    // Attack
    public float attackCooldown; // time left to attack


    public CoroutineExecutor coroutineExecutor;

    public BlackBoard(EntityData data, CoroutineExecutor coroutineExecutor, Team team)
    {
        this.BaseData = data;
        this.coroutineExecutor = coroutineExecutor;
        this.team = team;
    }
}