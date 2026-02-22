using UnityEngine;

public class UnitDepositState : UnitStateBase
{
    private IUnitStateContext stateContext;
    private IResourceCarrier resourceCarrier;

    public UnitDepositState(UnitStateMachine stateMachine, BlackBoard blackBoard, IUnitStateContext stateContext, IResourceCarrier resourceCarrier) : base(stateMachine, blackBoard)
    {
        this.stateContext = stateContext;
        this.resourceCarrier = resourceCarrier;
    }

    public override void Enter()
    {
        if (!resourceCarrier.IsCarryingResources())
        {
            stateMachine.ChangeState<UnitIdleState>();
            return;
        }

        resourceCarrier.DepositResource(ResourceType.Gold);
        resourceCarrier.DepositResource(ResourceType.Wood);

        if(blackBoard.PreviousTarget.Entity is not ResourceProvider provider)
        {
            stateMachine.ChangeState<UnitIdleState>();
            return;
        }
        
        if(provider != null && provider.RemainingResource > 0)
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
