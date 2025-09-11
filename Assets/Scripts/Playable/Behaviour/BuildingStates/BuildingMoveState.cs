using UnityEngine;
using UnityEngine.AI;

public class BuildingMoveState : BuildingStateBase
{
    private NavMeshAgent agent;

    public BuildingMoveState(BuildingStateMachine stateMachine, BlackBoard blackBoard, NavMeshAgent agent) : base(stateMachine, blackBoard)
    {
        this.agent = agent;
    }

    public override void Enter()
    {
        agent.isStopped = false;
        agent.SetDestination(blackBoard.target.GetPosition());
    }

    public override void Exit()
    {
        agent.isStopped = true;
    }

    public override void Update()
    {
        if (!blackBoard.target.IsGround)
        {
            agent.SetDestination(blackBoard.target.GetPosition());

            if (agent.remainingDistance < blackBoard.data.AttackRange)
                stateMachine.ChangeState<BuildingIdleState>();

        }
        else
        {
            if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
                stateMachine.ChangeState<BuildingIdleState>();
        }
    }
}
