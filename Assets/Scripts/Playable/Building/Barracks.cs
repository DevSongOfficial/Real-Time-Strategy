using NUnit.Framework;
using UnityEngine;


public interface IUnitGenerator
{
    void SetUnitGenerator(UnitGenerator unitGenerator);
}

public class Barracks : Building, IUnitGenerator
{
    private new BarracksData data;
    private UnitGenerator unitGenerator;

    [SerializeField] private Vector2 spawnPointOffset; // building.position + spawnPoint would be the spawn point.

    public void SetUnitGenerator(UnitGenerator unitGenerator)
    {
        this.unitGenerator = unitGenerator;
        //data = (BarracksData)base.data;
    }

    public override void ExecuteCommand(Command command)
    {
        base.ExecuteCommand(command);
        this.command = command;
        if (command.Type == CommandType.TrainUnit)
        {
            unitGenerator.Generate(command.entityToGenerate, team, transform.position + new Vector3(spawnPointOffset.x, 0, spawnPointOffset.y));
        }
    }
}