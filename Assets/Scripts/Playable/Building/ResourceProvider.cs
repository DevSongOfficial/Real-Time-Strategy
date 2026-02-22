using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

// Buildings that provide resources such as Gold, Wood, 
public sealed class ResourceProvider : Building
{
    // Registered Units(Harvesters).
    public List<IResourceCarrier> registeredUnits; // Units assigned to this mine/tree.
    public int RemainingUnitSlot => GetData().UnitSlotCount - registeredUnits.Count;

    // About Resource
    public event Action OnResourceDepleted;
    public int RemainingResource { get; private set; }

    protected override void Awake()
    {
        base.Awake();

        team = Team.None;

        registeredUnits = new List<IResourceCarrier>();
    }

    private void Start()
    {
        RemainingResource = GetData().TotalAmount;
    }

    // Should be called when a unit start harvesting.
    public void RegisterHarvester(IResourceCarrier resourceCarrier)
    {
        registeredUnits.Add(resourceCarrier);
    }

    public void UnregisterHarvester(IResourceCarrier resourceCarrier)
    {
        if(IsRegistered(resourceCarrier))
            registeredUnits.Remove(resourceCarrier);
    }

    public bool IsRegistered(IResourceCarrier resourceCarrier)
    {
        return registeredUnits.Contains(resourceCarrier);
    }

    public int TakeResource()
    {
        var amountToTake = GetData().AmountPerAction;
        if (amountToTake > RemainingResource)
            amountToTake = RemainingResource;
        
        RemainingResource -= amountToTake;

        if (RemainingResource <= 0)
            HandleResourceDepleted();

        return amountToTake;
    }

    public override string GetProgressLabelName()
    {
        return $"{RemainingResource} / {GetData().TotalAmount}";
    }

    public override float GetProgressRate()
    {
        return (float)RemainingResource / GetData().TotalAmount;
    }

    public IEnumerable<IResourceCarrier> GetRegisteredUnits()
    {
        return registeredUnits;
    }

    private void HandleResourceDepleted()
    {
        if(GetData().DestroyOnResourceDepleted)
            StartCoroutine(DestroyRoutine());

        OnResourceDepleted?.Invoke();
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