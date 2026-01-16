using UnityEngine;
using System;

public class BuildingUnderConstructionState: BuildingStateBase
{
    private float leftTime;
    public event Action OnFinished;

    private bool isPaused;

    public BuildingUnderConstructionState(BuildingStateMachine stateMachine, BuildingBlackBoard blackBoard) : base(stateMachine, blackBoard)
    {
        leftTime = blackBoard.BaseData.ConsructionTime;
    }

    public override void Enter()
    {
        isPaused = false;
    }

    public override void Exit()
    {
        OnFinished?.Invoke();
        OnFinished = null;
    }

    public override void Update()
    {
        if (isPaused)
            return;

        leftTime -= Time.deltaTime;
        if (leftTime <= 0)
        {
            stateMachine.ChangeState<BuildingIdleState>();
        }
    }

    public void PauseConstruction()
    {
        isPaused = true;
        OnFinished = null;
    }
}
