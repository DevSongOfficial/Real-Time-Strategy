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
        stateContext.DepositResource(ResourceType.Gold);
        stateContext.DepositResource(ResourceType.Wood);

        stateMachine.ChangeState<UnitIdleState>();
    }

    public override void Exit()
    {
        
    }
}
