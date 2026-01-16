using UnityEngine;

public class BuildingUnitTrainState : BuildingStateBase
{
    private new BuildingBlackBoard blackBoard;
    private IUnitGenerator generator;
    private float leftTime;

    // TODO: Move this to Unit SO
    private float timeToGenerateAUnit = 4;

    public BuildingUnitTrainState(BuildingStateMachine stateMachine, BuildingBlackBoard blackBoard, IBuildingStateContext stateContext) : 
        base(stateMachine, blackBoard, stateContext)
    {
        generator = stateContext.GetUnitGenerator();
        this.blackBoard = blackBoard;
    }

    public override void Enter()
    {
        leftTime = timeToGenerateAUnit;
    }

    public override void Exit()
    {
    }

    public override void Update()
    {
        leftTime -= Time.deltaTime;
        blackBoard.progressRate = 1 - (leftTime / timeToGenerateAUnit);

        if(leftTime <= 0)
        {
            blackBoard.progressRate = 0;

            generator.GenerateUnit(blackBoard.unitGenerationInfo);
            stateMachine.ChangeState<BuildingIdleState>();
        }
    }
}
