using System.Collections;
using UnityEngine;

// Buildings that provide resources such as Gold, Wood, Food
public sealed class ResourceProvider : Building
{
    protected override void Awake()
    {
        base.Awake();

        team = Team.None;
    }

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

        if(RemainingAmount <= 0 && GetData().DestroyOnResourceDepleted)
            StartCoroutine(DestroyRoutine());

        return amountToTake;
    }

    public override string GetProgressLabelName()
    {
        return $"{RemainingAmount} / {GetData().TotalAmount}";
    }

    public override float GetProgressRate()
    {
        return (float)RemainingAmount / GetData().TotalAmount;
    }

    private IEnumerator DestroyRoutine()
    {
        yield return null;

        gameObject.SetActive(false);
    }

    public new ResourceProviderData GetData()
    {
        return base.GetData() as ResourceProviderData;
    }
}