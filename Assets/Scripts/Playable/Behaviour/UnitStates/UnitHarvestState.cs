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
        if (blackBoard.target.Entity is not ResourceProvider building)
        {
            stateMachine.ChangeState<UnitIdleState>();
            return;
        }

        this.building = building;
        harvestTime = building.GetData().TimeToHarvest;
    }

    public override void Update()
    {
        base.Update();

        harvestTime -= Time.deltaTime;
        if (harvestTime <= 0)
        {
            // Harvest and carry the resource.
            var resourceType = building.GetData().ResourceType;
            var amount = building.GetData().AmountPerAction;
            stateContext.CarryResource(resourceType, amount);

            blackBoard.target = new Target(Player.HQ);
            stateMachine.ChangeState<UnitMoveState>();
            return;
        }
    }

    public override void Exit()
    {
    }
}