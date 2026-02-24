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

    public void Generate(UnitData unitData, Team team, Vector3 spawnPosition, Vector3? rallyPoint = null)
    {
        if (!HasCapacityFor(unitData)) return;

        var newUnit = unitFactory.Create(unitData, team);
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

    public void GenerateWithRandomPosition(UnitData unitData, Team team, int numberOfUnit = 1)
    {
        for (int i = 0; i < numberOfUnit; i++)
        {
            var randomPosition = new Vector3(Random.Range(26, 35), 2, Random.Range(20, 36));
            Generate(unitData, team, randomPosition);
        }
    }
}

public class UnitGenerationInfo
{
    public UnitData Data;
    public Team team;
    public Vector3 rallyPosition;

    public UnitGenerationInfo(UnitData unitData, Team team, Vector3 position)
    {
        this.Data = unitData; 
        this.team = team; 
        this.rallyPosition = position;
    }
}