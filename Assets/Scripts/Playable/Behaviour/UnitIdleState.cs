using UnityEngine;
using UnityEngine.AI;

public class UnitIdleState : StateBase
{
    private NavMeshAgent agent;

    public UnitIdleState(StateMachine stateMachine, BlackBoard blackBoard,  NavMeshAgent agent)
        : base(stateMachine, blackBoard)
    {
        this.agent = agent;
    }

    public override void Enter()
    {
        if(agent != null) 
            agent.isStopped = true;
    }

    public override void Exit()
    {
        if (agent != null)
            agent.isStopped = false;
    }

    public override void Update()
    {
        // Do some funny things
    }
}
