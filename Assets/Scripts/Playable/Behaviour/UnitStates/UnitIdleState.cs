using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.AI;
using System.Security.Cryptography.X509Certificates;

public class UnitIdleState : UnitStateBase
{
    private Animator animator;

    public UnitIdleState(UnitStateMachine stateMachine, BlackBoard blackBoard, Animator animator)
        : base(stateMachine, blackBoard)
    {
        this.animator = animator;
    }

    public override void Enter()
    {
        animator.Play("Breathing Idle", 0, 0f);
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
