using UnityEngine;

public class UnitConstructState : UnitStateBase
{
    private IUnitStateContext stateContext;
    private float timeLeft;

    

    public UnitConstructState(UnitStateMachine stateMachine, BlackBoard blackBoard, IUnitStateContext stateContext)
        : base(stateMachine, blackBoard)
    {
        this.stateContext = stateContext;
    }

    public override void Enter()
    {
        timeLeft = blackBoard.constructionTime;
        stateContext.CrossFadeAnimation("Construct", 0.3f, 0);
    }

    public override void Exit()
    {

    }

    public override void Update()
    {
        base.Update();
        
        timeLeft -= Time.deltaTime;
        Debug.Log("Constructing...");
        if(timeLeft <= 0)
        {
            // 건물 완성하면 실행할 코드...
            Debug.Log("Complete!");
            blackBoard.OnConstructionFinished?.Invoke();
            blackBoard.OnConstructionFinished = null;
            stateMachine.ChangeState<UnitIdleState>();
        }
    }
}
