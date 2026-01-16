using UnityEngine;

public class UnitConstructState : UnitStateBase
{
    private IUnitStateContext stateContext;

    private Building building; // Building to construct.

    public UnitConstructState(UnitStateMachine stateMachine, BlackBoard blackBoard, IUnitStateContext stateContext)
        : base(stateMachine, blackBoard)
    {
        this.stateContext = stateContext;
    }

    public override void Enter()
    {
        if (blackBoard.target.Entity is not Building building)
        {
            stateMachine.ChangeState<UnitIdleState>();
            return;
        }

        this.building = building;
        building.StartConstruction(SwitchToIdleState);

        stateContext.CrossFadeAnimation("Construct", 0.05f, 0);
        stateContext.LookAt(blackBoard.target.GetPosition());
    }

    public override void Exit() 
    {
        building.PauseConstruction();
    }

    public override void Update()
    {
        base.Update();
    }

    private void SwitchToIdleState()
    {
        stateMachine.ChangeState<UnitIdleState>();
    }
}