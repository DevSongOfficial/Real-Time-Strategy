using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


public interface IUnitGenerator
{
    void SetUnitGenerator(UnitGenerator unitGenerator);
    void GenerateUnit(UnitGenerationInfo unitGenerationInfo);

    void EnqueueUnit(UnitGenerationInfo unitInfo);
    UnitGenerationInfo DequeueUnit();
    UnitGenerationInfo PeekNextUnit();
    int GetUnitCount();
}

public class Barracks : Building, ITarget<BarracksData>, IUnitGenerator
{
    private UnitGenerator unitGenerator;
    private Queue<UnitGenerationInfo> unitGenerationQueue; // Waiting list for units to be generated

    [SerializeField] private Vector2 spawnPointOffset; // building.position + spawnPoint would be the spawn point.


    public override void ExecuteCommand(CommandData command)
    {
        if(IsUnderConstruction)
            return;

        if (GetUnitCount() >= GetData().GenerationSlotCount) 
            return;

        base.ExecuteCommand(command);

        if (command is UnitTrainCommandData unitTrainCommand)
        {
            var unitGenerationInfo = new UnitGenerationInfo(
                unitTrainCommand.UnitData, 
                team, 
                transform.position + new Vector3(spawnPointOffset.x, 0, spawnPointOffset.y));

            EnqueueUnit(unitGenerationInfo);

            stateMachine.ChangeState<BuildingUnitTrainState>();
        }
    }

    public void SetUnitGenerator(UnitGenerator unitGenerator)
    {
        if (unitGenerator == null) return;

        this.unitGenerator = unitGenerator;
        unitGenerationQueue = new Queue<UnitGenerationInfo>();
    }

    public void GenerateUnit(UnitGenerationInfo info)
    {
        unitGenerator.Generate(info.Data, info.team, info.spawnPosition);
    }

    public void EnqueueUnit(UnitGenerationInfo unitInfo)
    {
        var maxCount = GetData().GenerationSlotCount;
        if (GetUnitCount() >= maxCount) return;

        unitGenerationQueue.Enqueue(unitInfo);
    }

    public UnitGenerationInfo DequeueUnit()
    {
        if(GetUnitCount() == 0) return null;

        return unitGenerationQueue.Dequeue();
    }

    public UnitGenerationInfo PeekNextUnit()
    {
        if (GetUnitCount() == 0) return null;

        return unitGenerationQueue.Peek();
    }

    public int GetUnitCount() => unitGenerationQueue.Count;

    public override string GetProgressLabelName()
    {
        if(stateMachine.CurrentState is BuildingUnitTrainState)
            return $"Traning {PeekNextUnit().Data.DisplayName}...";

        return base.GetProgressLabelName();
    }

    public override IEnumerable<Sprite> GetTraningUnitSprites()
    {
        if (stateMachine.CurrentState is not BuildingUnitTrainState)
            yield break;

        foreach (var unit in unitGenerationQueue)
            yield return unit.Data.ProfileSprite; 
    }

    public new BarracksData GetData()
    {
        return base.GetData() as BarracksData;
    }

    public override IUnitGenerator GetUnitGenerator()
    {
        return this;
    }

}