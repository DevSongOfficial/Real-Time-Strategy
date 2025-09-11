using UnityEngine;
using UnityEngine.AI;

public interface IMovable
{
    void MoveTo(Vector3 destination);
}

[RequireComponent(typeof(NavMeshAgent))]
public class Unit : Playable, IMovable, IDamageable, ITargetor, ITarget
{
    // State Machine
    private UnitStateMachine stateMachine;
    private BlackBoard blackBoard;

    [SerializeField] private NavMeshAgent agent;

    [SerializeField] private GameObject selectionIndicator;

    private HealthSystem healthSystem;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    public Unit SetUp(EntityData data)
    {
        this.data = data;

        blackBoard = new BlackBoard(data);
        stateMachine = new UnitStateMachine(agent, blackBoard);

        healthSystem = new HealthSystem(data.MaxHealth);

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
        stateMachine.ChangeState(stateMachine.MoveState);
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