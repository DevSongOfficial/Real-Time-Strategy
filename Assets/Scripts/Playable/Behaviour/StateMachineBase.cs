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
    public UnitIdleState        IdleState { get; private set; }
    public UnitMoveState        MoveState { get; private set; }
    public UnitAttackState      AttackState { get; private set; }
    public UnitConstructState   ConstructState { get; private set; }
    public UnitHarvestState     HarvestState {  get; private set; }
    public UnitDepositState     DepositState { get; private set; }

    private BlackBoard blackBoard;
    private IUnitStateContext stateContext;

    public UnitStateMachine(IUnitStateContext stateContext, BlackBoard blackBoard)
    {
        this.blackBoard = blackBoard;
        this.stateContext = stateContext;

        IdleState       = new UnitIdleState(this, blackBoard, stateContext);
        MoveState       = new UnitMoveState(this, blackBoard, stateContext);
        AttackState     = new UnitAttackState(this, blackBoard, stateContext);
        ConstructState  = new UnitConstructState(this, blackBoard, stateContext);
        HarvestState    = new UnitHarvestState(this, blackBoard, stateContext);
        DepositState    = new UnitDepositState(this, blackBoard, stateContext);

        RegisterState(IdleState);
        RegisterState(MoveState);
        RegisterState(AttackState);
        RegisterState(ConstructState);
        RegisterState(HarvestState);
        RegisterState(DepositState);

        // Initial State
        ChangeState<UnitIdleState>();
    }

    public UnitStateBase DetermineNextState()
    {
        Target target = blackBoard.Target;

        if (target.IsGround)
            return IdleState;

        if (target.Entity.GetTeam() != blackBoard.team)
        {
            if (target.Entity is IDamageable)
                return AttackState;
        }
        
        
        if (target.Entity is ResourceProvider) // means the target is GoldMine or Tree
            return HarvestState;
        else if (target.Entity is Building)
        {
            if (target.Entity is HeadQuarters)
                return DepositState;

            return ConstructState;
        }

        return IdleState;
    }
}

public class BuildingStateMachine : StateMachineBase
{
    public BuildingIdleState                IdleState { get; private set; }
    public BuildingUnderConstructionState   ConstructionState { get; private set; }
    public BuildingUnitTrainState           UnitTrainState { get; private set; }

    public BuildingStateMachine(IBuildingStateContext stateContext, BuildingBlackBoard blackBoard)
    {
        // Default State
        ConstructionState       = new BuildingUnderConstructionState(this, blackBoard, stateContext);

        // States after construction's done.
        IdleState               = new BuildingIdleState(this, blackBoard, stateContext);
        UnitTrainState          = new BuildingUnitTrainState(this, blackBoard, stateContext);

        RegisterState(IdleState);
        RegisterState(ConstructionState);
        RegisterState(UnitTrainState);

        // Start default state, but have it wait til a unit start construction.
        ChangeState<BuildingUnderConstructionState>();
        ConstructionState.PauseConstruction();
    }
}

public class HybridBuildingStateMachine : BuildingStateMachine
{
    public HybridBuildingStateMachine(IBuildingStateContext stateContext, BuildingBlackBoard blackBoard, NavMeshAgent agent) : base(stateContext, blackBoard)
    {
        RegisterState(new BuildingMoveState(this, blackBoard, agent, stateContext));
    }
}


public class BlackBoard
{
    public EntityData BaseData { get; private set; }
    public Team team;
    public HeadQuarters hq;

    // Move
    public Target Target { get; private set; }
    public Target PreviousTarget { get; private set; }
    public void SetTarget(Target target)
    {
        PreviousTarget = Target;
        Target = target;
    }

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

public class BuildingBlackBoard : BlackBoard
{
    public new BuildingData BaseData => base.BaseData as BuildingData;

    public float progressRate; // 0 ~ 1

    public BuildingBlackBoard(EntityData data, CoroutineExecutor coroutineExecutor, Team team)
        :base(data, coroutineExecutor, team)
    {
        
    }
}