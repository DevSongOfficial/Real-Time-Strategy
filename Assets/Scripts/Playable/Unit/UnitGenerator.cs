using CustomResourceManagement;
using Unity.AppUI.UI;
using UnityEngine;

public class UnitGenerator
{
    public System.Action<Unit> OnUnitGenerated;

    private UnitFactory unitFactory;
    IUnitRegisterer unitRegistry;

    public UnitGenerator(UnitFactory unitFactory, IUnitRegisterer unitRegistry)
    {
        this.unitFactory = unitFactory;
        this.unitRegistry = unitRegistry;
    }

    public void Generate(EntityData unitData, Team team, Vector3 position)
    {
        var newUnit = unitFactory.Create(unitData, team);
        newUnit.SetPosition(position);
        unitRegistry.RegisterUnit(newUnit);

        OnUnitGenerated?.Invoke(newUnit);
    }

    public void RandomGenerate(EntityData unitData, int numberOfUnit = 1)
    {
        for (int i = 0; i < numberOfUnit; i++)
        {
            var randomPosition = new Vector3(Random.Range(26, 35), 2, Random.Range(20, 36));
            Generate(unitData, i > numberOfUnit / 2 ? Team.Green : Team.Red, randomPosition);
        }
    }
}

public struct UnitGenerationInfo
{
    public UnitData unitData;
    public Team team;
    public Vector3 spawnPosition;

    public UnitGenerationInfo(UnitData unitData, Team team, Vector3 position)
    {
        this.unitData = unitData; 
        this.team = team; 
        this.spawnPosition = position;
    }
}