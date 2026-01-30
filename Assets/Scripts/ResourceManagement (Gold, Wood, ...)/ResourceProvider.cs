using UnityEngine;

// Buildings that provide resources such as Gold, Wood, Food
public sealed class ResourceProvider : Building
{
    private void Start()
    {
        RemainingAmount = GetData().TotalAmount;
    }

    public int RemainingAmount { get; private set; }

    public int TakeResource()
    {
        var amountToTake = GetData().AmountPerAction;
        if (amountToTake > RemainingAmount)
            amountToTake = RemainingAmount;
        
        RemainingAmount -= amountToTake;

        return amountToTake;
    }

    public new ResourceProviderData GetData()
    {
        return base.GetData() as ResourceProviderData;
    }
}