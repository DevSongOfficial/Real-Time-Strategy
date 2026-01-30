using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class UnitMoveState : UnitStateBase
{
    private IUnitStateContext stateContext;

    public UnitMoveState(UnitStateMachine stateMachine, BlackBoard blackBoard, IUnitStateContext stateContext)
        : base(stateMachine, blackBoard)
    {
        this.stateContext = stateContext;
    }

    public override void Enter()
    {
        stateContext.SetDestination(blackBoard.Target.GetPosition());

        if (stateMachine.PreviousState != stateMachine.CurrentState)
            stateContext.CrossFadeAnimation("Run", 0.1f, 0);
    }

    public override void Exit()
    {
        stateContext.ClearDestination();
    }

    public override void Update()
    {
        base.Update();

        var nextState = stateMachine.DetermineNextState();

        if (nextState == stateMachine.AttackState)
        {
            var contactDistance = stateContext.CaculateContactDistance(blackBoard.Target);
            stateContext.SetDestination(blackBoard.Target.GetPosition());
            if (stateContext.GetRemainingDistance() - contactDistance * 0.5f < blackBoard.BaseData.Combat.AttackRange)
            {
                if (blackBoard.attackCooldown <= 0)
                {
                    stateMachine.ChangeState<UnitAttackState>();
                }
            }

            return;
        }

        if (nextState == stateMachine.ConstructState)
        {
            var contactDistance = stateContext.CaculateContactDistance(blackBoard.Target);
            if (stateContext.HasArrived(contactDistance * 0.5f))
            {
                stateMachine.ChangeState<UnitConstructState>();
            }

            return;
        }

        if(nextState == stateMachine.HarvestState)
        {
            var contactDistance = stateContext.CaculateContactDistance(blackBoard.Target);
            if (stateContext.HasArrived(contactDistance * 0.5f))
            {
                stateMachine.ChangeState<UnitHarvestState>();
            }
        }

        if(nextState == stateMachine.DepositState)
        {
            var contactDistance = stateContext.CaculateContactDistance(blackBoard.Target);
            if (stateContext.HasArrived(contactDistance * 0.5f))
            {
                stateMachine.ChangeState<UnitDepositState>();
            }
        }

        if(nextState == stateMachine.IdleState)
        {
            if(stateContext.HasArrived())
                stateMachine.ChangeState<UnitIdleState>();
        }
    }
}