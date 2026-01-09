using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.AI;
using System.Security.Cryptography.X509Certificates;

public class UnitIdleState : UnitStateBase
{
    private IUnitStateContext stateContext;
    public UnitIdleState(UnitStateMachine stateMachine, BlackBoard blackBoard, IUnitStateContext stateContext)
        : base(stateMachine, blackBoard)
    {
        this.stateContext = stateContext;

    }

    public override void Enter()
    {
        stateContext.CrossFadeAnimation("Breathing Idle", 0.05f, 0);
    }

    public override void Exit()
    {
  
    }

    public override void Update()
    {
        base.Update();
        // Do some funny things
    }
}
