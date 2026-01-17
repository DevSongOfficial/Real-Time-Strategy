using UnityEngine;

public class BuildingUnitTrainState : BuildingStateBase
{
    private new BuildingBlackBoard blackBoard;
    private IUnitGenerator generator;
    private float generationTime;
    private float leftTime;

    public BuildingUnitTrainState(BuildingStateMachine stateMachine, BuildingBlackBoard blackBoard, IBuildingStateContext stateContext) : 
        base(stateMachine, blackBoard, stateContext)
    {
        generator = stateContext.GetUnitGenerator();
        this.blackBoard = blackBoard;
    }

    public override void Enter()
    {
        generationTime = generator.PeekNextUnit().Data.TrainingTime;
        leftTime = generationTime;
    }

    public override void Exit()
    {
        blackBoard.progressRate = 0;
    }

    public override void Update()
    {
        if(generator.GetUnitCount() == 0)
        {
            stateMachine.ChangeState<BuildingIdleState>();
            return;
        }

        leftTime -= Time.deltaTime;
        blackBoard.progressRate = 1 - (leftTime / generationTime);

        if (leftTime > 0) return;

        var unitInfo = generator.DequeueUnit();
        generator.GenerateUnit(unitInfo);

        if(generator.GetUnitCount() > 0)
            stateMachine.ChangeState<BuildingUnitTrainState>();
        else
            stateMachine.ChangeState<BuildingIdleState>();
    }
}