using UnityEngine;
using UnityEngine.AI;

// This building can Move & Attack just like units.
public class HybridBuilding : Building, ITargetor
{
    [SerializeField] private NavMeshAgent agent;

    protected override void Awake()
    {
        base.Awake();

        if (agent == null)
            agent = GetComponent<NavMeshAgent>();

        if(agent.enabled == false)
            agent.enabled = true;

        // If you wanna control the rotation or position by yourself, not through navmeshagent, 
        // you can use things like:
        // agent.updateUpAxis = false;
        // agent.updateRotation = false;
        // agent.updatePosition = false;
    }

    public override Building SetUp(EntityData data, GameObject selectionIndicator)
    {
        this.data = data;

        blackBoard = new BlackBoard(data);
        stateMachine = new HybridBuildingStateMachine(blackBoard, agent);

        healthSystem = new HealthSystem(data.MaxHealth);

        this.selectionIndicator = selectionIndicator;

        return this;
    }

    public void SetTarget(Target target)
    {
        blackBoard.target = target;
        stateMachine.ChangeState<BuildingMoveState>();
    }

    public override void MakeRenderOnly()
    {
        agent.enabled = false;
        base.MakeRenderOnly();
    }
}