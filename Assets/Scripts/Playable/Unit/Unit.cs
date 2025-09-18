using UnityEngine;
using UnityEngine.AI;


[RequireComponent(typeof(NavMeshAgent))]
public class Unit : Playable, IDamageable, ITargetor, ITarget
{
    // State Machine
    private UnitStateMachine stateMachine;
    private BlackBoard blackBoard;

    [SerializeField] private NavMeshAgent agent;

    private GameObject selectionIndicator;

    private HealthSystem healthSystem;

    private void Awake()
    {
        if(agent == null)
            agent = GetComponent<NavMeshAgent>();
    }

    public Unit SetUp(EntityData data, GameObject selectionIndicator)
    {
        this.data = data;

        blackBoard = new BlackBoard(data);
        stateMachine = new UnitStateMachine(agent, blackBoard);

        healthSystem = new HealthSystem(data.MaxHealth);

        this.selectionIndicator = selectionIndicator;

        return this;
    }

    private void Update()
    {
        stateMachine.Update();
    }

    public override void OnSelected()
    {
        selectionIndicator.SetActive(false);
        selectionIndicator.SetActive(true);
    }

    public override void OnDeselected()
    {
        selectionIndicator.SetActive(false);
    }

    public void SetTarget(Target target)
    {
        blackBoard.target = target;
        stateMachine.ChangeState<UnitMoveState>();
    }

    public Vector3 GetPosition() => transform.position;
    public IHealthSystem GetHealthSystem() => healthSystem;

    public void MoveTo(Vector3 destination)
    {
        agent?.SetDestination(destination);
    }

    public void GetDamaged(int damage)
    {
        healthSystem.GetDamaged(damage);
    }
}