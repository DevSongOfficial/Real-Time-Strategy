using UnityEngine;

public class BuildingUnitTrainState : BuildingStateBase
{
    private IUnitGenerator generator;
    private new BuildingBlackBoard blackBoard;

    public BuildingUnitTrainState(BuildingStateMachine stateMachine, BuildingBlackBoard blackBoard, IBuildingStateContext stateContext) : 
        base(stateMachine, blackBoard, stateContext)
    {
        generator = stateContext.GetUnitGenerator();
        this.blackBoard = blackBoard;
    }

    public override void Enter()
    {
        generator.GenerateUnit(blackBoard.unitGenerationInfo);
    }

    public override void Exit()
    {
    }

    public override void Update()
    {
    }
}
