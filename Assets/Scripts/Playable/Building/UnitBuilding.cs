using UnityEngine;
using UnityEngine.AI;

public class UnitBuilding : Building, IMovable, ITarget, ITargetor
{
    [SerializeField] private NavMeshAgent agent;


    public IHealthSystem GetHealthSystem()
    {
        throw new System.NotImplementedException();
    }

    public Vector3 GetPosition()
    {
        throw new System.NotImplementedException();
    }

    public void MoveTo(Vector3 destination)
    {
        agent?.SetDestination(destination);
    }

    public void SetTarget(Target target)
    {
        blackBoard.target = target;
        stateMachine.ChangeState(stateMachine.MoveState);
    }
}
