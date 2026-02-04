using UnityEngine;

public class Worker : Unit, IResourceCarrier, IConstructor
{
    public override void ExecuteCommand(CommandData command)
    {
        base.ExecuteCommand(command);

        // When Place() is called:
        if (command is BuildCommandData)
            placementEvent.OnPlacementRequested += StartConstruction;
    }

    private void StartConstruction(ITarget building)
    {
        placementEvent.OnPlacementRequested -= StartConstruction; // one shot handler 

        blackBoard.SetTarget(new Target(building));

        stateMachine.ChangeState<UnitMoveState>();
    }

    public void CarryResource(ResourceType type, int amount)
    {
        resourceBank.AddResource(type, amount);
    }

    public int DepositResource(ResourceType type)
    {
        var depositAmount = resourceBank.GetResourceAmount(type);
        resourceBank.SpendResource(type, depositAmount);

        return depositAmount;
    }

    public bool IsCarryingResources()
    {
        return resourceBank.GetResourceAmount(ResourceType.Gold) > 0
            || resourceBank.GetResourceAmount(ResourceType.Wood) > 0;
    }
}