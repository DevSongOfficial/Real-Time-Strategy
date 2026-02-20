using UnityEngine;

public class UnitDieState : UnitStateBase
{
    private IUnitStateContext stateContext;
    public UnitDieState(UnitStateMachine stateMachine, BlackBoard blackBoard, IUnitStateContext stateContext)
        : base(stateMachine, blackBoard)
    {
        this.stateContext = stateContext;

    }

    public override void Enter()
    {
        stateMachine.DisableStateChanges();
        stateContext.CrossFadeAnimation(blackBoard.BaseData.Combat.DieAnimation, 0.05f, 0);
    }

    public override void Exit()
    {

    }

    public override void Update()
    {
        base.Update();
    }
}
