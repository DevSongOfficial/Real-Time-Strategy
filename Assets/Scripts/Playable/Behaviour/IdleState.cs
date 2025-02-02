using UnityEngine;
using UnityEngine.AI;

public class IdleState : UnitStateBase
{
    private NavMeshAgent agent;

    public IdleState(UnitStateMachine stateMachine, BlackBoard blackBoard,  NavMeshAgent agent)
        : base(stateMachine, blackBoard)
    {
        this.agent = agent;
    }

    public override void Enter()
    {
        agent.isStopped = true;
    }

    public override void Exit()
    {
        agent.isStopped = false;
    }

    public override void Update()
    {
        
    }
}
