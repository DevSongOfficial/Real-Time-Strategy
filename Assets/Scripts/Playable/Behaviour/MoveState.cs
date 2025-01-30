using UnityEngine;
using UnityEngine.AI;

public class MoveState : StateBase
{
    private StateMachine stateMachine;
    private NavMeshAgent agent;
    private Target target;

    public MoveState(StateMachine stateMachine, NavMeshAgent agent, Target target)
    {
        this.stateMachine = stateMachine;
        this.target = target;
        this.agent = agent;
    }

    public override void Enter()
    {
        agent.SetDestination(target.GetPosition());
    }

    public override void Exit()
    {
        
    }

    public override void Update()
    {
        if (!target.IsGround)
            agent.SetDestination(target.GetPosition());

        if (agent.remainingDistance < 3)
        {
            stateMachine.ChangeState(new IdleState(agent));
        }

        if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
            stateMachine.ChangeState(new IdleState(agent));

    }
}
