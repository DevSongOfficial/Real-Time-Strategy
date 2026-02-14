using UnityEngine;

public class BuildingUnitTrainState : BuildingStateBase
{
    private new BuildingBlackBoard blackBoard;
    
    private IUnitGenerator generator;
    private float generationTime;

    private UnitGenerationInfo previouslyRequestedUnit;
    private float leftTime;

    public BuildingUnitTrainState(BuildingStateMachine stateMachine, BuildingBlackBoard blackBoard, IBuildingStateContext stateContext) : 
        base(stateMachine, blackBoard, stateContext)
    {
        generator = stateContext.GetUnitGenerator();
        this.blackBoard = blackBoard;
    }

    public override void Enter()
    {
        UnitGenerationInfo newlyRequestedUnit = generator.PeekNextUnit();
        if (newlyRequestedUnit == previouslyRequestedUnit)
            return;

        generationTime = newlyRequestedUnit.Data.TrainingTime;
        leftTime = generationTime;

        previouslyRequestedUnit = newlyRequestedUnit;
    }

    public override void Exit()
    {
        blackBoard.progressRate = 0;
    }

    public override void Update()
    {
        if(generator.GetUnitCountInQueue() == 0)
        {
            stateMachine.ChangeState<BuildingIdleState>();
            return;
        }

        if (!generator.CanGenerateUnit(generator.PeekNextUnit())) return;

        leftTime -= Time.deltaTime;
        blackBoard.progressRate = 1 - (leftTime / generationTime);

        if (leftTime > 0) return;

        GenerateUnit();

        SwitchToNextState();
    }

    private void GenerateUnit()
    {
        var unitInfo = generator.DequeueUnit();
        generator.GenerateUnit(unitInfo);
    }

    private void SwitchToNextState()
    {
        if (generator.GetUnitCountInQueue() > 0)
            stateMachine.ChangeState<BuildingUnitTrainState>();
        else
            stateMachine.ChangeState<BuildingIdleState>();
    }
}