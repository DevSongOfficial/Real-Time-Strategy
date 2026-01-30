using UnityEngine;

public class UnitDepositState : UnitStateBase
{
    private IUnitStateContext stateContext;

    public UnitDepositState(UnitStateMachine stateMachine, BlackBoard blackBoard, IUnitStateContext stateContext) : base(stateMachine, blackBoard)
    {
        this.stateContext = stateContext;
    }

    public override void Enter()
    {
        if (!stateContext.IsCarryingResources())
        {
            stateMachine.ChangeState<UnitIdleState>();
            return;
        }

        stateContext.DepositResource(ResourceType.Gold);
        stateContext.DepositResource(ResourceType.Wood);

        if(blackBoard.PreviousTarget.Entity is not ResourceProvider provider)
        {
            stateMachine.ChangeState<UnitIdleState>();
            return;
        }
        
        if(provider != null && provider.RemainingAmount > 0)
        {
            blackBoard.SetTarget(blackBoard.PreviousTarget);
            stateMachine.ChangeState<UnitMoveState>();
        }
        else
        {
            stateMachine.ChangeState<UnitIdleState>();
        }
    }

    public override void Exit()
    {
        
    }
}
