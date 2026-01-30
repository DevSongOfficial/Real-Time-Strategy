using UnityEngine;

public class UnitHarvestState : UnitStateBase
{
    private IUnitStateContext stateContext;
    private ResourceProvider building; // mine or tree
    private float harvestTime;

    public UnitHarvestState(UnitStateMachine stateMachine, BlackBoard blackBoard, IUnitStateContext stateContext) : base(stateMachine, blackBoard)
    {
        this.stateContext = stateContext;
    }

    public override void Enter()
    {
        if (blackBoard.Target.Entity is not ResourceProvider building ||
            building.RemainingAmount == 0)
        {
            stateMachine.ChangeState<UnitIdleState>();
            return;
        }

        this.building = building;
        harvestTime = building.GetData().TimeToHarvest;
        stateContext.CrossFadeAnimation("Dig", 0.5f, 0);
    }

    public override void Update()
    {
        base.Update();

        harvestTime -= Time.deltaTime;
        if (harvestTime > 0)
            return;

        HarvestResource();
        CarryResourceToHQ();
    }

    public override void Exit() { }

    private void HarvestResource()
    {
        var resourceType = building.GetData().ResourceType;
        var amount = building.TakeResource();
        stateContext.CarryResource(resourceType, amount);
    }

    private void CarryResourceToHQ()
    {
        blackBoard.SetTarget(new Target(Player.HQ));
        stateMachine.ChangeState<UnitMoveState>();
    }
}