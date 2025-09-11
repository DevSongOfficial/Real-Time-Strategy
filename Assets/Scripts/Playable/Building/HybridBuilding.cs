using UnityEngine;
using UnityEngine.AI;

// This building can Move & Attack like units.
public class HybridBuilding : Building, IMovable, ITargetor
{
    [SerializeField] private NavMeshAgent agent;

    private void Awake()
    {
        if (agent == null)
            agent = GetComponent<NavMeshAgent>();
    }

    public override Building SetUp(EntityData data)
    {
        this.data = data;

        blackBoard = new BlackBoard(data);
        stateMachine = new HybridBuildingStateMachine(blackBoard, agent);

        healthSystem = new HealthSystem(data.MaxHealth);

        return this;

    }

    public void MoveTo(Vector3 destination)
    {
        agent.SetDestination(destination);
    }

    public void SetTarget(Target target)
    {
        blackBoard.target = target;
        stateMachine.ChangeState<BuildingMoveState>();
    }
}