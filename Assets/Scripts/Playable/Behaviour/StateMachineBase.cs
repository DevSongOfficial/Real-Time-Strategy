using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

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
    public UnitStateMachine(Animator animator, NavMeshAgent agent, BlackBoard blackBoard)
    {
        RegisterState(new UnitIdleState(this, blackBoard, animator));
        RegisterState(new UnitMoveState(this, blackBoard, agent, animator));
        RegisterState(new UnitAttackState(this, blackBoard, animator));

        // Initial State
        ChangeState<UnitIdleState>();
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

    public Target target;
    public float attackCooldown; // time left to attack

    public CoroutineExecutor coroutineExecutor;

    public BlackBoard(EntityData data, CoroutineExecutor coroutineExecutor)
    {
        this.BaseData = data;
        this.coroutineExecutor = coroutineExecutor;
    }
}