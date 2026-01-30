using UnityEngine;
using UnityEngine.AI;
using static UnityEngine.GraphicsBuffer;

public class BuildingMoveState : BuildingStateBase
{
    private NavMeshAgent agent;

    public BuildingMoveState(BuildingStateMachine stateMachine, BlackBoard blackBoard, NavMeshAgent agent, IBuildingStateContext stateContext) : base(stateMachine, blackBoard, stateContext)
    {
        this.agent = agent;
    }

    public override void Enter()
    {
        Debug.Log($"enabled={agent.enabled}, isOnNavMesh={agent.isOnNavMesh}");
        agent.isStopped = false;
        agent.SetDestination(blackBoard.Target.GetPosition());
    }

    public override void Exit()
    {
        agent.isStopped = true;
    }

    public override void Update()
    {
        if (!blackBoard.Target.IsGround)
        {
            var contactDistance = blackBoard.Target.Entity.GetData().RadiusOnTerrain + blackBoard.BaseData.RadiusOnTerrain;
            agent.SetDestination(blackBoard.Target.GetPosition());

            if (agent.remainingDistance < contactDistance * 0.5f)
                stateMachine.ChangeState<BuildingIdleState>();

        }
        else
        {
            if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
                stateMachine.ChangeState<BuildingIdleState>();
        }
    }
}
