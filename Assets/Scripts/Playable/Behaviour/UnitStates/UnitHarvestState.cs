using System.Linq.Expressions;
using UnityEngine;

public class UnitHarvestState : UnitStateBase
{
    private IUnitStateContext stateContext;
    private IResourceCarrier resourceCarrier;

    private ResourceProvider resourceProvider; // mine or tree
    private float harvestTime;


    public UnitHarvestState(UnitStateMachine stateMachine, BlackBoard blackBoard, IUnitStateContext stateContext, IResourceCarrier resourceCarrier) : base(stateMachine, blackBoard)
    {
        this.stateContext = stateContext;
        this.resourceCarrier = resourceCarrier;
    }

    public override void Enter()
    {
        if (blackBoard.Target.Entity is not ResourceProvider resourceProvider ||
            resourceProvider.RemainingResource == 0 ||
            resourceProvider.RemainingUnitSlot == 0)
        {
            stateMachine.ChangeState<UnitIdleState>();
            return;
        }

        if(!resourceProvider.IsRegistered(resourceCarrier))
            AssignResourceProvider(resourceProvider);

        harvestTime = resourceProvider.GetData().TimeToHarvest;
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

    private void AssignResourceProvider(ResourceProvider resourceProvider)
    {
        this.resourceProvider = resourceProvider;
        resourceProvider.RegisterHarvester(resourceCarrier);
        
        resourceProvider.OnResourceDepleted += UnassignResourceProvider;
        stateMachine.OnStateChanged         += TryUnassignResourceProvider;
    }

    private void TryUnassignResourceProvider()
    {
        if (this.resourceProvider == null) return;

        if (blackBoard.Target.Entity is HeadQuarters)
            return;

        if (blackBoard.Target.Entity is ResourceProvider resourceProvider && resourceProvider == this.resourceProvider)
            return;

        UnassignResourceProvider();

        stateMachine.OnStateChanged -= TryUnassignResourceProvider;
    }

    private void UnassignResourceProvider()
    {
        this.resourceProvider.UnregisterHarvester(resourceCarrier);
        this.resourceProvider = null;
    }

    private void HarvestResource()
    {
        if(resourceProvider == null)
        {
            stateMachine.ChangeState<UnitIdleState>();
            return;
        }

        var resourceType = resourceProvider.GetData().ResourceType;
        var amount = resourceProvider.TakeResource();
        resourceCarrier.CarryResource(resourceType, amount);
    }

    private void CarryResourceToHQ()
    {
        blackBoard.SetTarget(new Target(Player.HQ));
        stateMachine.ChangeState<UnitMoveState>();
    }
}