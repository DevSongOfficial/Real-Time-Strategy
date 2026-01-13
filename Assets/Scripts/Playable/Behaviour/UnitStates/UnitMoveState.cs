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
        stateContext.SetDestination(blackBoard.target.GetPosition());

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
        Debug.Log(nextState);

        if(nextState == stateMachine.AttackState)
        {
            var contactDistance = stateContext.CaculateContactDistance(blackBoard.target);
            stateContext.SetDestination(blackBoard.target.GetPosition());
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
            var contactDistance = stateContext.CaculateContactDistance(blackBoard.target);
            if (stateContext.HasArrived(contactDistance * 0.5f))
            {
                stateMachine.ChangeState<UnitConstructState>();
            }

            return;
        }

        if(nextState == stateMachine.IdleState)
        {
            if(stateContext.HasArrived())
                stateMachine.ChangeState<UnitIdleState>();
        }
    }
}