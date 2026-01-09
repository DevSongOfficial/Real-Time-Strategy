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

        if (!blackBoard.target.IsGround)
        {
            stateContext.SetDestination(blackBoard.target.GetPosition());

            var contactDistance = blackBoard.target.Entity.GetData().RadiusOnTerrain + blackBoard.BaseData.RadiusOnTerrain;
            if (stateContext.GetRemainingDistance()- contactDistance * 0.5f < blackBoard.BaseData.Combat.AttackRange)
            {
                if (blackBoard.target.Entity is IDamageable)
                    if (blackBoard.attackCooldown <= 0)
                    {
                        stateContext.LookAt(blackBoard.target.GetPosition());
                        stateMachine.ChangeState<UnitAttackState>();
                    }

                // else Interact with the object;
            }
        }
        else
        {
            if (stateContext.HasArrived())
                stateMachine.ChangeState<UnitIdleState>();
        }
    }
}