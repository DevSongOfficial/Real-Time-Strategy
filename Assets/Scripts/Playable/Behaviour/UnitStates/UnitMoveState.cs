using UnityEngine;
using UnityEngine.AI;

public class UnitMoveState : UnitStateBase
{
    private NavMeshAgent agent;
    private Animator animator;

    public UnitMoveState(UnitStateMachine stateMachine, BlackBoard blackBoard, NavMeshAgent agent, Animator animator)
        : base(stateMachine, blackBoard)
    {
        this.agent = agent;
        this.animator = animator;
    }

    public override void Enter()
    {
        agent.isStopped = false;
        agent.SetDestination(blackBoard.target.GetPosition());

        if(stateMachine.PreviousState != stateMachine.CurrentState)
            animator.CrossFade("Run", 0.1f, 0);
    }

    public override void Exit()
    {
        agent.isStopped = true;
    }

    public override void Update()
    {
        base.Update();

        if (!blackBoard.target.IsGround)
        {
            var contactDistance = blackBoard.target.Entity.GetData().RadiusOnTerrain + blackBoard.data.RadiusOnTerrain;
            agent.SetDestination(blackBoard.target.GetPosition());

            if (agent.remainingDistance - contactDistance * 0.5f < blackBoard.data.AttackRange)
            {
                if (blackBoard.target.Entity is IDamageable)
                    if (blackBoard.attackCooldown <= 0)
                    {
                        stateMachine.ChangeState<UnitAttackState>();
                    }

                // else Interact with the object;
            }

        }
        else
        {
            if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
                stateMachine.ChangeState<UnitIdleState>();
        }
    }
}