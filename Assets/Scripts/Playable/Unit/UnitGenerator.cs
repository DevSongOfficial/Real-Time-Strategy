using CustomResourceManagement;
using Unity.AppUI.UI;
using UnityEngine;

public class UnitGenerator
{
    public System.Action<Unit> OnUnitGenerated;
    public System.Action<Unit> OnUnitDestroyed;
    public System.Action<Unit> OnUnitDeathRequested;

    private UnitFactory unitFactory;
    private IUnitRegisterer unitRegistry;
    private UnitCapacitySlots capacitySlots;

    public UnitGenerator(UnitFactory unitFactory, IUnitRegisterer unitRegistry, UnitCapacitySlots capacitySlots)
    {
        this.unitFactory = unitFactory;
        this.unitRegistry = unitRegistry;
        this.capacitySlots = capacitySlots;
    }

    public bool HasCapacityFor(UnitData unitData)
    {
        return capacitySlots.FreeCapacity >= unitData.CapacityCost;
    }

    public void Generate(UnitData unitData, TeamContext teamContext, Vector3 spawnPosition, Vector3? rallyPoint = null)
    {
        if (!HasCapacityFor(unitData)) return;

        var newUnit = unitFactory.Create(unitData, teamContext);
        newUnit.SetPosition(spawnPosition);
        
        unitRegistry.RegisterUnit(newUnit);
        newUnit.OnDeathRequested += OnUnitDeathRequested;
        newUnit.OnDestroyed += unitRegistry.UnregisterUnit;
        newUnit.OnDestroyed += OnUnitDestroyed;

        capacitySlots.Occupy(unitData.CapacityCost);

        if(rallyPoint.HasValue)
            newUnit.SetTarget(new Target(rallyPoint.Value));

        OnUnitGenerated?.Invoke(newUnit);
    }

    public void GenerateWithRandomPosition(UnitData unitData, TeamContext teamContext, int numberOfUnit = 1)
    {
        for (int i = 0; i < numberOfUnit; i++)
        {
            var randomPosition = new Vector3(Random.Range(26, 35), 2, Random.Range(20, 36));
            Generate(unitData, teamContext, randomPosition);
        }
    }
}

public class UnitGenerationInfo
{
    public UnitData Data;
    public TeamContext teamContext;
    public Vector3 rallyPosition;

    public UnitGenerationInfo(UnitData unitData, TeamContext teamContext, Vector3 position)
    {
        this.Data = unitData; 
        this.teamContext = teamContext; 
        this.rallyPosition = position;
    }
}