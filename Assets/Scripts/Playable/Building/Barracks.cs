using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


public interface IUnitGenerator
{
    void SetUnitGenerator(UnitGenerator unitGenerator);
    void SetSpawnPositionSetter(RallyPointSetter setter);

    void GenerateUnit(UnitGenerationInfo unitGenerationInfo);

    void EnqueueUnit(UnitGenerationInfo unitInfo);
    UnitGenerationInfo DequeueUnit();
    UnitGenerationInfo PeekNextUnit();
    int GetUnitCount();

    Vector3 GetUnitRallyPoint();
    void SetUnitRallyPoint(Vector3 spawnPosition);
}

public class Barracks : Building, ITarget<BarracksData>, IUnitGenerator
{
    private UnitGenerator unitGenerator;
    private Queue<UnitGenerationInfo> unitGenerationQueue; // Waiting list for units to be generated

    private RallyPointSetter rallyPointSetter;
    private Vector3 unitRallyPoint;
    private Vector3 unitSpawnPoint;
    
    public override Building SetUp(EntityData data, GameObject selectionIndicator, EntityProfilePanel profilePanel, Team team)
    {
        return base.SetUp(data, selectionIndicator, profilePanel, team);
    }

    public override void SetPosition(Vector3 position)
    {
        base.SetPosition(position);

        if(!IsPreview)
        {
            unitSpawnPoint = CalculateSpawnPosition(SpawnPositionType.Random);
            unitRallyPoint = unitSpawnPoint;
        }
    }

    public override void ExecuteCommand(CommandData command)
    {
        if(IsUnderConstruction)
            return;

        if (GetUnitCount() >= GetData().GenerationSlotCount) 
            return;

        base.ExecuteCommand(command);

        if (command is UnitTrainCommandData unitTrainCommand)
        {
            var unitGenerationInfo 
                = new UnitGenerationInfo(unitTrainCommand.UnitData, team, unitRallyPoint);

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

    public void SetSpawnPositionSetter(RallyPointSetter setter)
    {
        this.rallyPointSetter = setter;
    }

    public override void OnSelected()
    {
        base.OnSelected();

        rallyPointSetter.Setup(this);
    }

    public override void OnDeselected()
    {
        base.OnDeselected();
    }

    public void GenerateUnit(UnitGenerationInfo info)
    {
        unitGenerator.Generate(info.Data, info.team, unitSpawnPoint, unitRallyPoint);
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

    public void SetUnitRallyPoint(Vector3 position)
    {
        unitRallyPoint = position;
    }

    public Vector3 GetUnitRallyPoint() => unitRallyPoint;


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