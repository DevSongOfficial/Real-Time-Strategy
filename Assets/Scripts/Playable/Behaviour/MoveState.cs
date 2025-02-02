using UnityEngine;
using UnityEngine.AI;

public class MoveState : UnitStateBase
{
    private NavMeshAgent agent;

    public MoveState(UnitStateMachine stateMachine, BlackBoard blackBoard, NavMeshAgent agent)
        : base(stateMachine, blackBoard)
    {
        this.agent = agent;
    }

    public override void Enter()
    {
        agent.SetDestination(blackBoard.target.GetPosition());
    }

    public override void Exit()
    {
        
    }

    public override void Update()
    {
        if (!blackBoard.target.IsGround)
            agent.SetDestination(blackBoard.target.GetPosition());

        if (agent.remainingDistance < blackBoard.data.AttackRange)
        {
            stateMachine.ChangeState(stateMachine.AttackState);
        }

        if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
            stateMachine.ChangeState(stateMachine.IdleState);

    }
}
