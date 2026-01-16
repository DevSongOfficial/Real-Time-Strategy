using NUnit.Framework;
using UnityEngine;


public interface IUnitGenerator
{
    void SetUnitGenerator(UnitGenerator unitGenerator);
    public void GenerateUnit(UnitGenerationInfo unitGenerationInfo);
}

public class Barracks : Building, ITarget<BarracksData>, IUnitGenerator
{
    private UnitGenerator unitGenerator;

    [SerializeField] private Vector2 spawnPointOffset; // building.position + spawnPoint would be the spawn point.

    public override void ExecuteCommand(Command command)
    {
        if(!CanExecuteCommand)
            return;

        base.ExecuteCommand(command);
        this.command = command;
        if (command.Type == CommandType.TrainUnit)
        {
            var unitGenerationInfo = new UnitGenerationInfo(
                (UnitData)command.entityToGenerate, 
                team, 
                transform.position + new Vector3(spawnPointOffset.x, 0, spawnPointOffset.y));
            blackBoard.unitGenerationInfo = unitGenerationInfo;
            stateMachine.ChangeState<BuildingUnitTrainState>();

        }
    }

    public void SetUnitGenerator(UnitGenerator unitGenerator)
    {
        if(unitGenerator != null)
            this.unitGenerator = unitGenerator;
    }

    public void GenerateUnit(UnitGenerationInfo info)
    {
        unitGenerator.Generate(info.unitData, info.team, info.spawnPosition);
    }

    public new BarracksData GetData()
    {
        return (BarracksData)base.GetData();
    }

    public override IUnitGenerator GetUnitGenerator()
    {
        return this;
    }

}