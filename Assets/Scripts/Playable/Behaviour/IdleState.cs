using UnityEngine;
using UnityEngine.AI;

public class IdleState : StateBase
{
    private NavMeshAgent agent;

    public IdleState(NavMeshAgent agent)
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
