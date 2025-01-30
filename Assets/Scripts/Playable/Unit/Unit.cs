using CustomResourceManagement;
using UnityEngine;
using UnityEngine.AI;

public interface IMovable
{
    void MoveTo(Vector3 destination);
}

[RequireComponent(typeof(NavMeshAgent))]
public class Unit : Playable, IMovable, IAttackable, IDamageable, ITargetor
{
    private StateMachine stateMachine;

    [SerializeField] private NavMeshAgent agent;

    [SerializeField] private GameObject selectionIndicator;

    private HealthSystem healthSystem;

    private void Awake()
    {
        stateMachine = new StateMachine();
        agent = GetComponent<NavMeshAgent>();
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

    // States:
    // Idle, Move, Attack

    public void SetTarget(Target target)
    {
        stateMachine.ChangeState(new MoveState(stateMachine, agent, target));
    }

    public void MoveTo(Vector3 destination)
    {
        agent?.SetDestination(destination);
    }

    public void Attack(IDamageable damageable)
    {
        damageable.GetDamaged(data.AttackDamage);
    }

    public void GetDamaged(int damage)
    {
        healthSystem.GetDamaged(damage);
        Debug.Log("Get Damaged");
    }


    private void OnTriggerEnter(Collider other)
    {
        var damageable = other.GetComponent<IDamageable>();
        if (damageable != null)
            damageable.GetDamaged(data.AttackDamage);
    }
}