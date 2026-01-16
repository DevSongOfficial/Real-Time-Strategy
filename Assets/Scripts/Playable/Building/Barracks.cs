using NUnit.Framework;
using UnityEngine;


public interface IUnitGenerator
{
    void SetUnitGenerator(UnitGenerator unitGenerator);
}

public class Barracks : Building, ITarget<BarracksData>, IUnitGenerator
{
    private UnitGenerator unitGenerator;

    [SerializeField] private Vector2 spawnPointOffset; // building.position + spawnPoint would be the spawn point.

    public void SetUnitGenerator(UnitGenerator unitGenerator)
    {
        this.unitGenerator = unitGenerator;
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

    public new BarracksData GetData()
    {
        return (BarracksData)base.GetData();
    }

}